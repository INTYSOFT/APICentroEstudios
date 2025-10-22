using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using api_intiSoft.Models.CentroEstudios;
using api_intiSoft.Services.AnswerSheets;
using intiSoft;

namespace api_intiSoft.Controllers.CentroEstudios
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluacionRespuestumsController : ControllerBase
    {
        private const string AnswerSource = "OMR-PDF";

        private readonly ConecDinamicaContext _context;
        private readonly IAnswerSheetProcessor _answerSheetProcessor;
        private readonly ILogger<EvaluacionRespuestumsController> _logger;

        public EvaluacionRespuestumsController(
            ConecDinamicaContext context,
            IAnswerSheetProcessor answerSheetProcessor,
            ILogger<EvaluacionRespuestumsController> logger)
        {
            _context = context;
            _answerSheetProcessor = answerSheetProcessor;
            _logger = logger;
        }

        // GET: api/EvaluacionRespuestums
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EvaluacionRespuestum>>> GetEvaluacionRespuestum()
        {
            return await _context.EvaluacionRespuestum.ToListAsync();
        }

        // GET: api/EvaluacionRespuestums/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EvaluacionRespuestum>> GetEvaluacionRespuestum(int id)
        {
            var evaluacionRespuestum = await _context.EvaluacionRespuestum.FindAsync(id);

            if (evaluacionRespuestum == null)
            {
                return NotFound();
            }

            return evaluacionRespuestum;
        }

        // PUT: api/EvaluacionRespuestums/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvaluacionRespuestum(int id, EvaluacionRespuestum evaluacionRespuestum)
        {
            if (id != evaluacionRespuestum.Id)
            {
                return BadRequest();
            }

            _context.Entry(evaluacionRespuestum).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EvaluacionRespuestumExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/EvaluacionRespuestums
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EvaluacionRespuestum>> PostEvaluacionRespuestum(EvaluacionRespuestum evaluacionRespuestum)
        {
            _context.EvaluacionRespuestum.Add(evaluacionRespuestum);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvaluacionRespuestum", new { id = evaluacionRespuestum.Id }, evaluacionRespuestum);
        }

        // DELETE: api/EvaluacionRespuestums/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvaluacionRespuestum(int id)
        {
            var evaluacionRespuestum = await _context.EvaluacionRespuestum.FindAsync(id);
            if (evaluacionRespuestum == null)
            {
                return NotFound();
            }

            _context.EvaluacionRespuestum.Remove(evaluacionRespuestum);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("procesar")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Procesar(
            [FromForm(Name = "archivo")] IFormFile? archivo,
            int evaluacionProgramadaId,
            int seccionId,
            CancellationToken cancellationToken)
        {
            var file = archivo ?? Request.Form.Files.FirstOrDefault();
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "Debe adjuntar un archivo PDF con las hojas de respuestas." });
            }

            if (!string.Equals(Path.GetExtension(file.FileName), ".pdf", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new { message = "El archivo proporcionado debe estar en formato PDF." });
            }

            if (evaluacionProgramadaId <= 0 || seccionId <= 0)
            {
                return BadRequest(new { message = "Los parámetros evaluacionProgramadaId y seccionId son obligatorios." });
            }

            await using var buffer = new MemoryStream();
            await file.CopyToAsync(buffer, cancellationToken);
            buffer.Position = 0;

            IReadOnlyCollection<StudentAnswerSheet> sheets;

            try
            {
                sheets = await _answerSheetProcessor.ProcessAsync(buffer, cancellationToken);
            }
            catch (AnswerSheetProcessingException ex)
            {
                _logger.LogWarning(ex, "No se pudo procesar el archivo de respuestas subido.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado procesando la hoja de respuestas.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Ocurrió un error al procesar la hoja de respuestas." });
            }

            if (sheets.Count == 0)
            {
                return BadRequest(new { message = "No se detectaron hojas de respuestas válidas en el archivo." });
            }

            var version = await ResolveVersionAsync(evaluacionProgramadaId, seccionId, cancellationToken);

            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var summary = new List<object>();

                foreach (var sheet in sheets)
                {
                    var evaluacion = await GetOrCreateEvaluacionAsync(sheet.Dni, evaluacionProgramadaId, seccionId, cancellationToken);

                    var existing = await _context.EvaluacionRespuestum
                        .Where(r => r.EvaluacionId == evaluacion.Id && r.Version == version)
                        .ToListAsync(cancellationToken);

                    if (existing.Count > 0)
                    {
                        _context.EvaluacionRespuestum.RemoveRange(existing);
                        await _context.SaveChangesAsync(cancellationToken);
                    }

                    var entries = sheet.Answers.Select(answer =>
                    {
                        var respuesta = NormalizeAnswer(answer.Option);
                        return new EvaluacionRespuestum
                        {
                            EvaluacionId = evaluacion.Id,
                            Version = version,
                            PreguntaOrden = answer.QuestionNumber,
                            Respuesta = respuesta,
                            Fuente = AnswerSource,
                            Confianza = respuesta is null ? null : ToDecimal(answer.Confidence),
                            Activo = true,
                            FechaRegistro = DateTime.UtcNow,
                            EvaluacionProgramadaId = evaluacionProgramadaId,
                            SeccionId = seccionId,
                            AlumnoId = null,
                            DniAlumno = sheet.Dni
                        };
                    }).ToList();

                    _context.EvaluacionRespuestum.AddRange(entries);
                    await _context.SaveChangesAsync(cancellationToken);

                    summary.Add(new
                    {
                        dni = sheet.Dni,
                        totalRespuestas = entries.Count,
                        respuestas = entries.Select(e => new
                        {
                            pregunta = e.PreguntaOrden,
                            opcion = e.Respuesta,
                            confianza = e.Confianza
                        })
                    });
                }

                await transaction.CommitAsync(cancellationToken);

                return Ok(new
                {
                    hojasProcesadas = summary.Count,
                    version,
                    estudiantes = summary
                });
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        private async Task<int> ResolveVersionAsync(int evaluacionProgramadaId, int seccionId, CancellationToken cancellationToken)
        {
            var version = await _context.EvaluacionClave
                .Where(c => c.EvaluacionProgramadaId == evaluacionProgramadaId && (!c.SeccionId.HasValue || c.SeccionId == seccionId))
                .OrderByDescending(c => c.Vigente)
                .ThenByDescending(c => c.Version)
                .Select(c => (int?)c.Version)
                .FirstOrDefaultAsync(cancellationToken);

            return version ?? 1;
        }

        private async Task<Evaluacion> GetOrCreateEvaluacionAsync(string dni, int evaluacionProgramadaId, int seccionId, CancellationToken cancellationToken)
        {
            var alumno = await _context.Alumno.FirstOrDefaultAsync(a => a.Dni == dni, cancellationToken);
            if (alumno == null)
            {
                alumno = new Alumno
                {
                    Dni = dni,
                    Activo = true,
                    FechaRegistro = DateTime.UtcNow
                };
                _context.Alumno.Add(alumno);
                await _context.SaveChangesAsync(cancellationToken);
            }

            var evaluacion = await _context.Evaluacion
                .FirstOrDefaultAsync(e => e.EvaluacionProgramadaId == evaluacionProgramadaId && e.AlumnoId == alumno.Id, cancellationToken);

            if (evaluacion == null)
            {
                evaluacion = new Evaluacion
                {
                    EvaluacionProgramadaId = evaluacionProgramadaId,
                    AlumnoId = alumno.Id,
                    SeccionId = seccionId,
                    Activo = true,
                    FechaRegistro = DateTime.UtcNow
                };
                _context.Evaluacion.Add(evaluacion);
                await _context.SaveChangesAsync(cancellationToken);
            }
            else if (evaluacion.SeccionId != seccionId)
            {
                evaluacion.SeccionId = seccionId;
                _context.Evaluacion.Update(evaluacion);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return evaluacion;
        }

        private static string? NormalizeAnswer(string? option)
        {
            if (string.IsNullOrWhiteSpace(option))
            {
                return null;
            }

            var normalized = option.Trim().ToUpperInvariant();
            if (normalized is "?" or "-")
            {
                return null;
            }

            return normalized.Length > 0 ? normalized.Substring(0, 1) : null;
        }

        private static decimal? ToDecimal(double confidence)
        {
            var clamped = Math.Clamp(confidence, 0, 1);
            return Math.Round((decimal)clamped, 4, MidpointRounding.AwayFromZero);
        }

        private bool EvaluacionRespuestumExists(int id)
        {
            return _context.EvaluacionRespuestum.Any(e => e.Id == id);
        }
    }
}
