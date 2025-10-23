// EvaluacionRespuestumsController.cs
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using api_intiSoft.Models.CentroEstudios;
using Docnet.Core;
using Docnet.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenCvSharp;
using intiSoft;
using Docnet.Core.Converters;
using System.Runtime.InteropServices; // ← para Marshal.Copy

namespace api_intiSoft.Controllers.CentroEstudios
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluacionRespuestumsController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public EvaluacionRespuestumsController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // =========================
        // POST: api/EvaluacionRespuestums/upload
        // =========================
        [HttpPost("upload")]
        [RequestSizeLimit(100_000_000)] // 100 MB por seguridad
        public async Task<IActionResult> ProcesarPdf(
            IFormFile pdf,
            [FromQuery] int evaluacionProgramadaId,
            [FromQuery] int seccionId,
            CancellationToken ct)
        {
            if (pdf == null || pdf.Length == 0)
                return BadRequest("Debe adjuntar un archivo PDF no vacío.");

            if (!pdf.ContentType.Contains("pdf", StringComparison.OrdinalIgnoreCase) &&
                !Path.GetExtension(pdf.FileName).Equals(".pdf", StringComparison.OrdinalIgnoreCase))
                return BadRequest("El archivo debe ser un PDF.");

            // 1) Resolver EvaluacionId y Version a partir de evaluacion_programada
            var evalLookup = await TryGetEvaluacionYVersionAsync(evaluacionProgramadaId, ct);
            if (evalLookup == null)
                return NotFound($"No se encontró evaluacion_programada con id={evaluacionProgramadaId}.");

            int evaluacionId = evalLookup.Value.EvaluacionId;
            int version = evalLookup.Value.Version;

            // 2) Convertir PDF a páginas (imágenes) con Docnet.Core
            List<OmrPageResult> omrResultados = new();

            using var ms = new MemoryStream();
            await pdf.CopyToAsync(ms, ct);
            ms.Position = 0;

            //using var docReader = DocLib.Instance.GetDocReader(ms.ToArray(), new PageDimensions(0, 0)); // full res
                                                                                                        // ~A4 a 300 DPI aprox (2480x3508) – ajusta si necesitas más/menos calidad
            using var docReader = DocLib.Instance.GetDocReader(ms.ToArray(), new PageDimensions(2480, 3508));

            int pageCount = docReader.GetPageCount();

            var swTotal = Stopwatch.StartNew();
            for (int page = 0; page < pageCount; page++)
            {
                ct.ThrowIfCancellationRequested();
                using var pageReader = docReader.GetPageReader(page);

                int width = pageReader.GetPageWidth();
                int height = pageReader.GetPageHeight();

                // BGRA (4 canales)
                // BGRA (4 canales) - obtener RGBA desde Docnet                
                byte[] rawBytes = pageReader.GetImage(); // ← sin parámetros
                // Docnet da RGBA → creamos Mat y copiamos el buffer (sin overloads privados)
                using var matRgba = new Mat(height, width, MatType.CV_8UC4);
                Marshal.Copy(rawBytes, 0, matRgba.Data, rawBytes.Length);

                // Convertimos a BGRA para mantener consistencia con el pipeline
                using var matBgra = new Mat();
                Cv2.CvtColor(matRgba, matBgra, ColorConversionCodes.RGBA2BGRA);

                var omr = OmrEngine.ProcessPage(matBgra); // ← OMR core (DNI + respuestas)
                omrResultados.Add(new OmrPageResult
                {
                    PageIndex = page,
                    Dni = omr.Dni,
                    Answers = omr.Answers
                });

            }
            swTotal.Stop();

            // 3) Persistir en DB
            var now = DateTime.UtcNow;
            var nuevas = new List<EvaluacionRespuestum>(capacity: omrResultados.Sum(r => r.Answers.Count));

            foreach (var page in omrResultados)
            {
                // Validación DNI
                var dniAlumno = page.Dni;
                if (string.IsNullOrWhiteSpace(dniAlumno) || dniAlumno.Length != 8)
                {
                    // Puedes decidir ignorar o fallar duro; aquí ignoramos la página sin DNI válido
                    continue;
                }

                foreach (var a in page.Answers)
                {
                    var entidad = new EvaluacionRespuestum
                    {
                        EvaluacionId = evaluacionId,
                        Version = version,
                        PreguntaOrden = a.QuestionNumber,
                        Respuesta = a.SelectedLetter,   // "A".."E"
                        Fuente = "OMR-PDF-v1",
                        Confianza = a.Confidence.HasValue
                            ? Convert.ToDecimal(Math.Clamp(a.Confidence.Value, 0d, 1d), CultureInfo.InvariantCulture)
                            : null,
                        TiempoMarcaMs = null,
                        Activo = true,
                        FechaRegistro = now,
                        FechaActualizacion = now,
                        UsuaraioRegistroId = null,
                        UsuaraioActualizacionId = null,
                        EvaluacionProgramadaId = evaluacionProgramadaId,
                        SeccionId = seccionId,
                        AlumnoId = null,
                        DniAlumno = dniAlumno
                    };

                    nuevas.Add(entidad);
                }
            }

            if (nuevas.Count == 0)
                return BadRequest("No se detectaron respuestas válidas en el PDF (revise plantilla/ROIs/umbral y DNI).");

            _context.EvaluacionRespuestum.AddRange(nuevas);
            await _context.SaveChangesAsync(ct);

            return Created(string.Empty, new
            {
                file = pdf.FileName,
                pages = pageCount,
                inserted = nuevas.Count,
                alumnos = omrResultados.Where(x => !string.IsNullOrEmpty(x.Dni)).Select(x => x.Dni).Distinct().ToArray(),
                elapsedMs = swTotal.ElapsedMilliseconds
            });
        }

        // =========================
        // Utilidades
        // =========================

        private async Task<(int EvaluacionId, int Version)?> TryGetEvaluacionYVersionAsync(
            int evaluacionProgramadaId,
            CancellationToken ct)
        {

            return (0, 0);

            await using var conn = _context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync(ct);

            await using var cmd = conn.CreateCommand();
            // ⚠️ Ajusta el esquema/tabla/columnas si difiere.
            cmd.CommandText = @"
                SELECT evaluacion_id, version
                FROM academia.evaluacion_programada
                WHERE id = @id
                LIMIT 1;";
            var p = cmd.CreateParameter();
            p.ParameterName = "@id";
            p.Value = evaluacionProgramadaId;
            cmd.Parameters.Add(p);

            await using var reader = await cmd.ExecuteReaderAsync(ct);
            if (await reader.ReadAsync(ct))
            {
                int evalId = reader.GetInt32(0);
                int version = reader.GetInt32(1);
                return (evalId, version);
            }
            return null;
        }
    }

    // =========================
    // Modelos de intercambio OMR (internos)
    // =========================
    internal sealed class OmrPageResult
    {
        public int PageIndex { get; set; }
        public string? Dni { get; set; }
        public List<OmrAnswer> Answers { get; set; } = new();
    }

    internal sealed class OmrAnswer
    {
        public int QuestionNumber { get; set; }
        public string SelectedLetter { get; set; } = "";
        public double? Confidence { get; set; }
    }

    // =========================
    // Motor OMR (Plantilla por porcentajes)
    // Ajusta ROIs y N_PREGUNTAS a tu formato real
    // =========================
    internal static class OmrEngine
    {
        // ← Ajusta a tu plantilla (ensayar con 3-5 PDFs)
        private const int N_PREGUNTAS = 50;      // Número total de preguntas por hoja
        private static readonly string[] LETTERS = new[] { "A", "B", "C", "D", "E" };

        // ROIs por porcentajes respecto al tamaño de la página
        // Banda DNI (izquierda)
        private static readonly RectPercent ROI_DNI = new(0.03, 0.10, 0.14, 0.80);
        // Zona de preguntas (derecha)
        private static readonly RectPercent ROI_QA = new(0.22, 0.10, 0.72, 0.80);

        public static (string? Dni, List<OmrAnswer> Answers) ProcessPage(Mat pageBgra)
        {
            using var gray = new Mat();
            using var pageBgr = new Mat();
            Cv2.CvtColor(pageBgra, pageBgr, ColorConversionCodes.BGRA2BGR);
            Cv2.CvtColor(pageBgr, gray, ColorConversionCodes.BGR2GRAY);

            // Binarización robusta
            using var bin = new Mat();
            Cv2.Threshold(gray, bin, 0, 255, ThresholdTypes.Otsu | ThresholdTypes.BinaryInv);

            // 1) DNI
            string? dni = ExtractDni(bin);

            // 2) Respuestas
            var answers = ExtractAnswers(bin);

            return (dni, answers);
        }

        private static string? ExtractDni(Mat bin)
        {
            var roi = ToRect(bin, ROI_DNI);
            using var dniMat = new Mat(bin, roi);

            // 8 columnas (8 dígitos), 10 filas (0-9)
            int cols = 8;
            int rows = 10;

            int cellW = dniMat.Width / cols;
            int cellH = dniMat.Height / rows;

            var digits = new char[cols];

            for (int c = 0; c < cols; c++)
            {
                int bestRow = -1;
                double bestFill = 0.0;

                for (int r = 0; r < rows; r++)
                {
                    var cellRect = new Rect(c * cellW, r * cellH, cellW, cellH);
                    using var cell = new Mat(dniMat, cellRect);

                    double fill = ComputeFillRatio(cell);

                    if (fill > bestFill)
                    {
                        bestFill = fill;
                        bestRow = r;
                    }
                }

                // Umbral mínimo para considerar marcado (ajustar con muestras)
                if (bestRow >= 0 && bestFill > 0.08) // 8% píxeles negros
                {
                    digits[c] = (char)('0' + bestRow);
                }
                else
                {
                    // no confiable, invalidar
                    return null;
                }
            }

            return new string(digits);
        }

        private static List<OmrAnswer> ExtractAnswers(Mat bin)
        {
            var roi = ToRect(bin, ROI_QA);
            using var qaMat = new Mat(bin, roi);

            var results = new List<OmrAnswer>(N_PREGUNTAS);

            int rows = N_PREGUNTAS;
            int cols = 5; // A..E

            int rowH = qaMat.Height / rows;
            int colW = qaMat.Width / cols;

            for (int q = 0; q < rows; q++)
            {
                int questionNumber = q + 1;

                int bestCol = -1;
                double bestFill = 0.0;
                double[] fills = new double[cols];

                for (int opt = 0; opt < cols; opt++)
                {
                    var cellRect = new Rect(opt * colW, q * rowH, colW, rowH);
                    using var cell = new Mat(qaMat, cellRect);

                    // Para evitar que el texto del número "1." contamine, ignoramos un margen izquierdo
                    var innerRect = new Rect((int)(cellRect.Width * 0.15), (int)(cellRect.Height * 0.10),
                                             (int)(cellRect.Width * 0.70), (int)(cellRect.Height * 0.80));
                    innerRect = Clamp(innerRect, cellRect.Size);

                    using var inner = new Mat(qaMat, new Rect(cellRect.X + innerRect.X, cellRect.Y + innerRect.Y, innerRect.Width, innerRect.Height));

                    double fill = ComputeFillRatio(inner);
                    fills[opt] = fill;

                    if (fill > bestFill)
                    {
                        bestFill = fill;
                        bestCol = opt;
                    }
                }

                // Asegurar unicidad: el mejor supera a los demás y un umbral mínimo
                double second = fills.OrderByDescending(v => v).Skip(1).FirstOrDefault();
                bool uniqueEnough = bestFill > 0.10 && (bestFill - second) > 0.03; // ajustable

                if (bestCol >= 0 && uniqueEnough)
                {
                    results.Add(new OmrAnswer
                    {
                        QuestionNumber = questionNumber,
                        SelectedLetter = LETTERS[bestCol],
                        Confidence = Math.Clamp(bestFill, 0, 1)
                    });
                }
                else
                {
                    // Si es ambiguo o no marcado, puedes:
                    // - Omitir
                    // - Registrar como null/"" (aquí omitimos para no insertar basura)
                }
            }

            return results;
        }

        // =========================
        // Helpers
        // =========================

        private static double ComputeFillRatio(Mat binCell)
        {
            // binCell es binaria (0 o 255). Contamos píxeles "negros"
            int black = Cv2.CountNonZero(binCell); // ojo: CountNonZero cuenta !=0 (blanco si invertido)
            // Como usamos BinaryInv, lo negro es 255 → CountNonZero devuelve "relleno"
            int total = binCell.Rows * binCell.Cols;
            if (total <= 0) return 0;
            return (double)black / total;
        }

        private static Rect ToRect(Mat m, RectPercent rp)
        {
            int x = (int)(m.Cols * rp.X);
            int y = (int)(m.Rows * rp.Y);
            int w = (int)(m.Cols * rp.W);
            int h = (int)(m.Rows * rp.H);
            var r = new Rect(x, y, w, h);
            // Clamp
            r.X = Math.Clamp(r.X, 0, m.Cols - 1);
            r.Y = Math.Clamp(r.Y, 0, m.Rows - 1);
            r.Width = Math.Clamp(r.Width, 1, m.Cols - r.X);
            r.Height = Math.Clamp(r.Height, 1, m.Rows - r.Y);
            return r;
        }

        private static Rect Clamp(Rect r, Size s)
        {
            var x = Math.Clamp(r.X, 0, s.Width - 1);
            var y = Math.Clamp(r.Y, 0, s.Height - 1);
            var w = Math.Clamp(r.Width, 1, s.Width - x);
            var h = Math.Clamp(r.Height, 1, s.Height - y);
            return new Rect(x, y, w, h);
        }

        private readonly struct RectPercent
        {
            public readonly double X, Y, W, H;
            public RectPercent(double x, double y, double w, double h) { X = x; Y = y; W = w; H = h; }
        }
    }
}
