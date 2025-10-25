using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using api_intiSoft.Dto;
using api_intiSoft.Models.CentroEstudios;
using api_intiSoft.Services;
using ContrlAcademico;
using ContrlAcademico.Services;
using intiSoft;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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
        /// Sube un PDF con hojas de respuesta, calibra la grilla por página,
        /// lee DNI y 100 respuestas (A–E) y hace upsert en academia.evaluacion_respuesta.
        /// </summary>
        [HttpPost("procesar")]
        [Consumes("multipart/form-data")]
        [DisableRequestSizeLimit]
        public async Task<ActionResult<object>> ProcesarPdf([FromForm] ProcesarPdfRequest request)
        {
            if (request.Pdf is null || request.Pdf.Length == 0)
                return BadRequest("Debe adjuntar un PDF no vacío en el campo 'pdf'.");

            // 1) Cargar configuración compartida (dpi, regiones normalizadas, etc.)
            var contentRoot = Directory.GetCurrentDirectory();
            var cfgPath = Path.Combine(contentRoot, "config.json");
            var cfg = ConfigModel.Load(cfgPath);

            // 2) Ghostscript (ruta de DLL)
            var gsPath = _config["Ghostscript:DllPath"];
            if (string.IsNullOrWhiteSpace(gsPath) || !System.IO.File.Exists(gsPath))
                return BadRequest($"Ghostscript DLL no encontrada en 'Ghostscript:DllPath'. Valor: '{gsPath ?? "(vacío)"}'.");

            var renderer = new PdfRenderer(gsPath, cfg.Dpi);
            var corrector = new RotationCorrector();
            var dniExt = new DniExtractor(cfg);

            // 3) Guardar PDF temporalmente con extensión .pdf
            var tmp = Path.GetTempFileName();
            var tempPdfPath = Path.ChangeExtension(tmp, ".pdf");
            System.IO.File.Move(tmp, tempPdfPath);

            await using (var fs = System.IO.File.Create(tempPdfPath))
                await request.Pdf.CopyToAsync(fs);

            // 4) Renderizar páginas
            List<Bitmap> pages;
            try
            {
                pages = renderer.RenderPages(tempPdfPath);
            }
            catch (Exception ex)
            {
                TryDelete(tempPdfPath);
                return Problem($"Ghostscript no pudo abrir el PDF. Detalle: {ex.Message}");
            }

            if (pages is null || pages.Count == 0)
            {
                TryDelete(tempPdfPath);
                return BadRequest("No se pudo extraer ninguna página del PDF (DLL incorrecta, permisos o PDF dañado/protegido).");
            }

            // 5) Derivar EvaluacionId/Version desde EvaluacionProgramada si tu modelo lo expone
            int evaluacionId = 0, version = 1;
            var ep = await _context.Set<EvaluacionProgramadum>()
                                   .AsNoTracking()
                                   .FirstOrDefaultAsync(x => x.Id == request.EvaluacionProgramadaId);


            var resumen = new List<object>();
            using var tx = await _context.Database.BeginTransactionAsync();

            try
            {
                for (int i = 0; i < pages.Count; i++)
                {
                    using var raw = pages[i];
                    using var aligned = corrector.Correct(raw); // deskew/rotación leve

                    // --- DNI ---
                    var dniRaw = dniExt.Extract(aligned);
                    var dni = new string(dniRaw.Where(char.IsDigit).ToArray());


                    // --- Calibración por página (ya la tienes arriba) ---
                    var calibrator = new SheetCalibrator();
                    var gp = calibrator.Calibrate(aligned);

                    // ¡OJO!: tu OmrProcessor espera StartX/StartY en el PRIMER ÓVALO (A, fila 1),
                    // Dx = paso HORIZONTAL entre opciones A..E
                    // Dy = paso VERTICAL entre filas 1..25

                    double blockWidth = gp.BlockSpacing;     // ancho de bloque (columna de 25)
                    double dyRow = gp.DxRow;                 // separación VERTICAL entre filas
                    double dxOpt = gp.DyOption;              // separación HORIZONTAL entre A..E

                    // Coordenadas del centro del primer óvalo (pregunta 1, opción A)
                    double startXCenter = gp.Panel.X + blockWidth * 0.19; // 19% hacia adentro del bloque
                    double startYCenter = gp.Panel.Y + dyRow * 0.16;       // 16% hacia abajo

                    // Distancia entre centros del mismo ítem en columnas consecutivas
                    double blockStride = gp.BlockSpacing;                   // centro a centro entre bloques

                    int startX = (int)Math.Round(startXCenter);
                    int startY = (int)Math.Round(startYCenter);
                    int stride = (int)Math.Round(blockStride);

                    var grid = new ContrlAcademico.GridModel
                    {
                        StartX       = startX,
                        StartY       = startY,
                        Rows         = gp.RowsPerBlock,         // 25
                        Cols         = gp.ColsPerQuestion,      // 5
                        BlockCount   = gp.BlockCount,           // 4
                        Dx           = (int)Math.Round(dxOpt),
                        Dy           = (int)Math.Round(dyRow),
                        BubbleW      = (int)Math.Round(gp.BubbleW),
                        BubbleH      = (int)Math.Round(gp.BubbleH),
                        BlockSpacing = stride
                    };


                    // --- Crear OMR con umbrales recomendados ---
                    var omr = new OmrProcessor(
                        grid,
                        cfg.DniRegion,
                        fillThreshold: 0.40,
                        meanThreshold: 220, // valor base, el algoritmo ahora ajusta dinámicamente por página
                        deltaMin: 18         // tolerancia para diferencias pequeñas entre 1º y 2º opción
                    );

                    // --- Respuestas (100): 'A'..'E' o '-' (guardamos "X") ---
                    var answers = omr.Process(aligned);
                    if (answers == null || answers.Length == 0)
                        throw new InvalidOperationException($"No se pudieron leer respuestas en la página {i + 1}.");

                    // --- Upsert por (evaluacion_id, version, pregunta_orden) ---
                    int inserted = 0, updated = 0;
                    for (int q = 0; q < answers.Length; q++)
                    {
                        int pregunta = q + 1; // orden por columna (1–25, 26–50, 51–75, 76–100)
                        string resp = answers[q] == '-' ? "X" : answers[q].ToString();

                        var existente = await _context.EvaluacionRespuestum
                            .FirstOrDefaultAsync(x =>
                                x.EvaluacionId  == evaluacionId &&
                                x.Version       == version &&
                                x.PreguntaOrden == pregunta);

                        if (existente == null)
                        {
                            _context.EvaluacionRespuestum.Add(new EvaluacionRespuestum
                            {
                                EvaluacionId            = evaluacionId,
                                Version                 = version,
                                PreguntaOrden           = pregunta,
                                Respuesta               = resp,
                                Fuente                  = "OMR",
                                Activo                  = true,
                                FechaRegistro           = DateTime.UtcNow,
                                FechaActualizacion      = DateTime.UtcNow,
                                EvaluacionProgramadaId  = request.EvaluacionProgramadaId,
                                SeccionId               = request.SeccionId,
                                AlumnoId                = null,
                                DniAlumno               = string.IsNullOrWhiteSpace(dni) ? null : dni
                            });
                            inserted++;
                        }
                        else
                        {
                            existente.Respuesta              = resp;
                            existente.DniAlumno              = string.IsNullOrWhiteSpace(dni) ? existente.DniAlumno : dni;
                            existente.EvaluacionProgramadaId = request.EvaluacionProgramadaId;
                            existente.SeccionId              = request.SeccionId;
                            existente.Fuente                 = "OMR";
                            existente.Activo                 = true;
                            existente.FechaActualizacion     = DateTime.UtcNow;
                            updated++;
                        }
                    }

                    resumen.Add(new
                    {
                        pagina = i + 1,
                        dni = string.IsNullOrWhiteSpace(dni) ? "(no leído)" : dni,
                        umbralUtilizado = Math.Round(omr.LastThreshold, 1),
                        insertados = inserted,
                        actualizados = updated
                    });
                }

                await _context.SaveChangesAsync();
                await tx.CommitAsync();
                return Ok(new { paginas = resumen.Count, detalle = resumen });
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

        // Helper para limpiar temporales con seguridad
        private static void TryDelete(string? path)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(path) && System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
            }
            catch
            {
                // no-op
            }
        }




    }




}
