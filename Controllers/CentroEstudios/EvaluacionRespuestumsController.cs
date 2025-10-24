using api_intiSoft.Models.CentroEstudios;
using ContrlAcademico;
using ContrlAcademico.Services;
using intiSoft;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace api_intiSoft.Controllers.CentroEstudios
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluacionRespuestumsController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;
        private readonly IConfiguration _config;

        public EvaluacionRespuestumsController(ConecDinamicaContext context, IConfiguration config)
        {
            _context = context;
            _config  = config;
        }

        /// <summary>
        /// Procesa un PDF de hojas de respuesta:
        /// - Lee DNI (8 dígitos marcados a la izquierda)
        /// - Lee 100 respuestas (A–E) en 4 bloques de 25
        /// - Inserta/actualiza en academia.evaluacion_respuesta
        /// </summary>
        /// <param name="pdf">Archivo PDF</param>
        /// <param name="evaluacionProgramadaId">Id de la evaluación programada</param>
        /// <param name="seccionId">Id de la sección</param>
        [HttpPost("procesar")]
        [DisableRequestSizeLimit]
        public async Task<ActionResult<object>> ProcesarPdf(
            IFormFile pdf,
            [FromQuery] int evaluacionProgramadaId,
            [FromQuery] int seccionId)
        {
            if (pdf == null || pdf.Length == 0)
                return BadRequest("Debe adjuntar un PDF no vacío.");

            // Cargar config.json (misma que usas en WinForms)
            var contentRoot = Directory.GetCurrentDirectory();
            var cfgPath = Path.Combine(contentRoot, "config.json");
            var cfg = ConfigModel.Load(cfgPath);

            // Crear utilidades
            var gsPath = _config["Ghostscript:DllPath"] ?? @"C:\Program Files\gs\gs10.05.1\bin\gsdll64.dll";
            //var renderer = new PdfRenderer(gsPath, cfg.Dpi);
            var renderer = new ContrlAcademico.Services.PdfRenderer(gsPath, cfg.Dpi);

            var corrector = new RotationCorrector();
            var omr = new OmrProcessor(cfg.AnswersGrid, cfg.DniRegion, fillThreshold: 0.5, meanThreshold: 180, deltaMin: 30);
            var dniReader = new DniExtractor(cfg); // OMR 8×10 en región normalizada

            // Derivar EvaluacionId y Version desde EvaluacionProgramada (si tu modelo lo expone)
            int evaluacionId = 0;
            int version = 1;
            try
            {
                var ep = await _context.Set<EvaluacionProgramadum>()
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(x => x.Id == evaluacionProgramadaId);
                if (ep != null)
                {
                    // Ajusta estas propiedades si el nombre difiere en tu modelo real
                    evaluacionId = ep.EvaluacionId;
                    version      = ep.Version;
                }
            }
            catch
            {
                // Si no existe la entidad o propiedades, continúa con defaults (o expón como params)
            }

            // Guardar PDF temporalmente
            var tempPdfPath = Path.GetTempFileName();
            await using (var fs = System.IO.File.Create(tempPdfPath))
            {
                await pdf.CopyToAsync(fs);
            }

            var resumen = new List<object>();
            using var tx = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1) Renderizar páginas
                var pages = renderer.RenderPages(tempPdfPath);

                for (int pageIndex = 0; pageIndex < pages.Count; pageIndex++)
                {
                    using Bitmap raw = pages[pageIndex];

                    // 2) Deskew/rotación
                    //using Bitmap aligned = corrector.Correct(raw);
                    using Bitmap aligned = corrector.Correct(raw) ?? (Bitmap)raw.Clone();

                    if (aligned == null)                  // por si tu Correct pudiera devolver null
                        return Problem("No se pudo alinear la página (RotationCorrector.Correct devolvió null).");


                    // 3) DNI (8 dígitos; si hay '-', lo dejamos tal cual o lo filtramos)                    
                    var dniRaw = dniReader.Extract(aligned);
                    var dni = new string(dniRaw.Where(char.IsDigit).ToArray()); // sólo dígitos

                    // 4) Respuestas (100)
                    var answers = omr.Process(aligned);   // 'A'..'E' o '-'


                    if (answers == null || answers.Length == 0)
                        throw new InvalidOperationException($"No se pudieron leer respuestas en la página {pageIndex + 1}.");

                    // 5) Upsert por clave única (evaluacion_id, version, pregunta_orden)
                    int inserted = 0, updated = 0;
                    for (int i = 0; i < answers.Length; i++)
                    {
                        int preguntaOrden = i + 1;
                        string respuesta = answers[i] == '-' ? "X" : answers[i].ToString();

                        var existente = await _context.EvaluacionRespuestum
                            .FirstOrDefaultAsync(x =>
                                x.EvaluacionId   == evaluacionId &&
                                x.Version        == version &&
                                x.PreguntaOrden  == preguntaOrden);

                        if (existente == null)
                        {
                            var nuevo = new EvaluacionRespuestum
                            {
                                EvaluacionId            = evaluacionId,
                                Version                 = version,
                                PreguntaOrden           = preguntaOrden,
                                Respuesta               = respuesta,
                                Fuente                  = "OMR",
                                Confianza               = null,
                                TiempoMarcaMs           = null,
                                Activo                  = true,
                                FechaRegistro           = DateTime.UtcNow,
                                FechaActualizacion      = DateTime.UtcNow,
                                UsuaraioRegistroId      = null,
                                UsuaraioActualizacionId = null,
                                EvaluacionProgramadaId  = evaluacionProgramadaId,
                                SeccionId               = seccionId,
                                AlumnoId                = null,
                                DniAlumno               = string.IsNullOrWhiteSpace(dni) ? null : dni
                            };

                            _context.EvaluacionRespuestum.Add(nuevo);
                            inserted++;
                        }
                        else
                        {
                            // Actualizamos campos variables (respuesta, DNI, sellos)
                            existente.Respuesta          = respuesta;
                            existente.DniAlumno          = string.IsNullOrWhiteSpace(dni) ? existente.DniAlumno : dni;
                            existente.EvaluacionProgramadaId = evaluacionProgramadaId;
                            existente.SeccionId          = seccionId;
                            existente.Activo             = true;
                            existente.Fuente             = "OMR";
                            existente.FechaActualizacion = DateTime.UtcNow;
                            updated++;
                        }
                    }

                    resumen.Add(new
                    {
                        pagina = pageIndex + 1,
                        dni = string.IsNullOrWhiteSpace(dni) ? "(no leído)" : dni,
                        insertados = inserted,
                        actualizados = updated
                    });
                }

                await _context.SaveChangesAsync();
                await tx.CommitAsync();

                return Ok(new
                {
                    paginas = resumen.Count,
                    detalle = resumen
                });
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                return Problem($"Error procesando PDF: {ex.Message}");
            }
            finally
            {
                TryDelete(tempPdfPath);
            }
        }

        private static void TryDelete(string path)
        {
            try { if (System.IO.File.Exists(path)) System.IO.File.Delete(path); }
            catch { /* no-op */ }
        }
    }

    // Suponemos que existe esta entidad en tu Modelo (como indica tu navegación)
    // Si ya la tienes definida en tu proyecto, borra esta clase "placeholder".
    public class EvaluacionProgramadum
    {
        public int Id { get; set; }
        public int EvaluacionId { get; set; }
        public int Version { get; set; }
    }
}
