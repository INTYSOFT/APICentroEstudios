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

            //using var docReader = DocLib.Instance.GetDocReader(ms.ToArray(), new PageDimensions(2480, 3508));
            // A5 landscape ~300 DPI
            //using var docReader = DocLib.Instance.GetDocReader(ms.ToArray(), new PageDimensions(2480, 1748));
            /*using var docReader = DocLib.Instance.GetDocReader(
                ms.ToArray(),
                new PageDimensions(1748, 2480));*/

            // A5 horizontal (~300 dpi)
            using var docReader = DocLib.Instance.GetDocReader(
            ms.ToArray(),
            new PageDimensions(1748, 2480)); // obligatoriamente así





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
                // Docnet → BGRA
                byte[] rawBytes = pageReader.GetImage();           // Docnet entrega BGRA en esta sobrecarga
                using var matBgra = new Mat(height, width, MatType.CV_8UC4);
                Marshal.Copy(rawBytes, 0, matBgra.Data, rawBytes.Length);

                // ⬅️ Rota aquí si ves que está en portrait (vertical) pero tu hoja es landscape
                //if (matBgra.Rows > matBgra.Cols)                    Cv2.Rotate(matBgra, matBgra, RotateFlags.Rotate90Clockwise);


                // Normalizamos la página (deskew + warp a A4 canónico) antes del OMR
                using var normalized = OmrEngine.NormalizePage(matBgra);

                // OMR en lienzo normalizado (DNI + respuestas)
                var omr = OmrEngine.ProcessPage(normalized);



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

                var ansCount = page.Answers?.Count ?? 0;

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
        // Lienzo objetivo A4 ~300dpi (estable, facilita porcentajes exactos)
        //private const int CANVAS_W = 2480;
        //private const int CANVAS_H = 3508;

        // A5 horizontal a ~300 dpi
        private const int CANVAS_W = 2480; // ancho
        private const int CANVAS_H = 1748; // alto

        // Banda DNI (izquierda): x=3%, w=15%, y=8%, h=84%                
        //private static readonly RectPercent ROI_DNI = new(0.070, 0.165, 0.215, 0.510);
        // x=7.0%  y=16.8%  w=21.5%  h=46%
        private static readonly RectPercent ROI_DNI = new(0.070, 0.165, 0.215, 0.510);

        // Zona de respuestas (derecha): x=21%, w=76%, y=8%, h=84%        
        private static readonly RectPercent ROI_QA = new(0.330, 0.085, 0.630, 0.860);        

        // Geometría de preguntas
        private const int COLS_QUEST = 4;     // 4 columnas
        private const int ROWS_PER_COL = 25;  // 25 por columna → 100 preguntas
        private const int OPTIONS = 5;        // A..E
        private static readonly string[] LETTERS = new[] { "A", "B", "C", "D", "E" };      

        private const double MARK_MIN_FILL = 0.10;  // ya lo dejaste así
        private const double MARK_GAP_BEST = 0.03;  // gap más permisivo
        private const double INNER_MARGIN = 0.14;  // un pelín menos de recorte
        private const double CIRCLE_RADIUS = 0.42;  // algo más grande en A5

        // ==============
        // Normalización
        // ==============
        public static Mat NormalizePage(Mat pageBgra)
        {
            using var bgr = new Mat();
            Cv2.CvtColor(pageBgra, bgr, ColorConversionCodes.BGRA2BGR);

            using var gray = new Mat();
            Cv2.CvtColor(bgr, gray, ColorConversionCodes.BGR2GRAY);

            using var edges = new Mat();
            Cv2.GaussianBlur(gray, gray, new Size(5, 5), 0);
            Cv2.Canny(gray, edges, 50, 150);

            var contours = Cv2.FindContoursAsArray(edges, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            // ✅ Si no hay contornos, NO rotamos, sólo reescalamos.
            if (contours.Length == 0)
                return bgr.Resize(new Size(CANVAS_W, CANVAS_H));

            var biggest = contours.OrderByDescending(cnt => Cv2.ContourArea(cnt)).First();
            var peri = Cv2.ArcLength(biggest, true);
            var approx = Cv2.ApproxPolyDP(biggest, 0.02 * peri, true);

            // ✅ Comprueba que “parece la hoja completa”
            double area = Cv2.ContourArea(biggest);
            double pageArea = bgr.Rows * bgr.Cols;
            bool looksLikePage = approx.Length == 4 && (area / pageArea) >= 0.88;

            if (looksLikePage)
            {
                var quad = SortQuad(approx.Select(p => new Point2f(p.X, p.Y)).ToArray());
                var dst = new[]
                {
            new Point2f(0, 0),
            new Point2f(CANVAS_W - 1, 0),
            new Point2f(CANVAS_W - 1, CANVAS_H - 1),
            new Point2f(0, CANVAS_H - 1),
        };
                using var M = Cv2.GetPerspectiveTransform(quad, dst);
                var warped = new Mat();
                Cv2.WarpPerspective(bgr, warped, M, new Size(CANVAS_W, CANVAS_H),
                                    InterpolationFlags.Linear, BorderTypes.Constant);
                // ❌ NO rotar aquí.
                return warped;
            }

            // ❌ Si el mayor contorno no es la hoja, NO warpear (evitas bandas/rotaciones aparentes)
            return bgr.Resize(new Size(CANVAS_W, CANVAS_H));
        }


        public static Mat old_NormalizePage(Mat pageBgra)
        {
            using var bgr = new Mat();
            Cv2.CvtColor(pageBgra, bgr, ColorConversionCodes.BGRA2BGR);

            using var gray = new Mat();
            Cv2.CvtColor(bgr, gray, ColorConversionCodes.BGR2GRAY);

            using var edges = new Mat();
            Cv2.GaussianBlur(gray, gray, new Size(5, 5), 0);
            Cv2.Canny(gray, edges, 50, 150);

            var contours = Cv2.FindContoursAsArray(edges, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
            if (contours.Length == 0)
            {
                var fallback = bgr.Clone();

                // ❌ NO rotamos más. Suponemos que viene correctamente orientado.
                // if (fallback.Rows > fallback.Cols)
                //     Cv2.Rotate(fallback, fallback, RotateFlags.Rotate90Clockwise);

                return fallback.Resize(new Size(CANVAS_W, CANVAS_H));
            }

            var biggest = contours.OrderByDescending(cnt => Cv2.ContourArea(cnt)).First();
            var peri = Cv2.ArcLength(biggest, true);
            var approx = Cv2.ApproxPolyDP(biggest, 0.02 * peri, true);

            var src = bgr;
            if (approx.Length == 4)
            {
                var quad = SortQuad(approx.Select(p => new Point2f(p.X, p.Y)).ToArray());
                var dst = new[]
                {
            new Point2f(0,0),
            new Point2f(CANVAS_W-1, 0),
            new Point2f(CANVAS_W-1, CANVAS_H-1),
            new Point2f(0, CANVAS_H-1),
        };
                using var M = Cv2.GetPerspectiveTransform(quad, dst);
                var warped = new Mat();
                Cv2.WarpPerspective(bgr, warped, M, new Size(CANVAS_W, CANVAS_H), InterpolationFlags.Linear, BorderTypes.Constant);
                src = warped;
            }

            // ❌ NO rotamos más.
            // if (src.Rows > src.Cols)
            //     Cv2.Rotate(src, src, RotateFlags.Rotate90Clockwise);

            return src.Resize(new Size(CANVAS_W, CANVAS_H));
        }       


        private static Point2f[] SortQuad(Point2f[] pts)
        {
            // top-left = suma mínima; bottom-right = suma máxima; top-right = diff negativa; bottom-left = diff positiva
            var sum = pts.Select(p => p.X + p.Y).ToArray();
            var diff = pts.Select(p => p.X - p.Y).ToArray();

            var tl = pts[Array.IndexOf(sum, sum.Min())];
            var br = pts[Array.IndexOf(sum, sum.Max())];
            var tr = pts[Array.IndexOf(diff, diff.Min())];
            var bl = pts[Array.IndexOf(diff, diff.Max())];

            return new[] { tl, tr, br, bl };
        }

        // ==============
        // Proceso OMR
        // ==============
        public static (string? Dni, List<OmrAnswer> Answers) ProcessPage(Mat pageBgrNormalized)
        {
            using var gray = new Mat();
            Cv2.CvtColor(pageBgrNormalized, gray, ColorConversionCodes.BGR2GRAY);

            using var bin = new Mat();
            int win = Math.Max(3, Math.Min(99, (Math.Min(gray.Rows, gray.Cols) / 80)));
            if ((win & 1) == 0) win++;
            if (win < 15) win = 15;

            Cv2.AdaptiveThreshold(
                gray, bin, 255,
                AdaptiveThresholdTypes.GaussianC,
                ThresholdTypes.BinaryInv,
                win, 7);

            using var closeK = Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(3, 3));
            using var openK = Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(2, 2));
            Cv2.MorphologyEx(bin, bin, MorphTypes.Close, closeK, iterations: 2);
            Cv2.MorphologyEx(bin, bin, MorphTypes.Open, openK, iterations: 1);

            // >>> pasa pageBgrNormalized para pintar el debug
            string? dni = ExtractDni(bin, pageBgrNormalized);
            var answers = ExtractAnswers(bin);

            return (dni, answers);
        }




        // Auto-localiza el panel del DNI usando densidad de píxeles "marcados" (255 en binaria invertida)

        // ROI auto-localizada del panel DNI para A5 horizontal normalizado.
        // Acotamos el rectángulo de búsqueda para evitar: (a) el rótulo "DNI DEL ESTUDIANTE",
        // (b) el bloque "MÓDULO/FIRMA/APELLIDOS" y (c) los marcadores de pie.
        // ROI auto-localizada del panel DNI mediante contorno rectangular + heurísticas.
        // Funciona en el lienzo A5 horizontal normalizado (2480×1748).
        private static Rect? TryAutoLocateDniRoi(Mat bin)
        {
            // Limitamos la búsqueda a la banda izquierda y por debajo del rótulo
            // Para tu hoja: el rótulo + línea manuscrita ocupan ~16–18% de alto.
            int sx = (int)(bin.Cols * 0.02);   // 2% desde la izquierda
            int sy = (int)(bin.Rows * 0.18);   // 18% desde arriba (debajo del texto)
            int sw = (int)(bin.Cols * 0.28);   // ancho suficiente para caja DNI
            int sh = (int)(bin.Rows * 0.62);   // hasta antes de MODULO/FIRMA/etc.

            sx = Math.Clamp(sx, 0, bin.Cols - 2);
            sy = Math.Clamp(sy, 0, bin.Rows - 2);
            sw = Math.Clamp(sw, 2, bin.Cols - sx);
            sh = Math.Clamp(sh, 2, bin.Rows - sy);

            var searchRect = new Rect(sx, sy, sw, sh);
            using var search = new Mat(bin, searchRect);

            // Bordeado para resaltar cajas
            using var edges = new Mat();
            Cv2.Canny(search, edges, 60, 160);
            Cv2.Dilate(edges, edges, Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3)), iterations: 1);

            // Contornos candidatos
            var contours = Cv2.FindContoursAsArray(edges, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
            if (contours.Length == 0) return null;

            // Heurísticas del panel DNI
            // - Debe ser más alto que ancho (relación ~ 1.25 alto/ancho)
            // - Área razonable (para 2480×1748 suele estar entre 6e4 y 3e5)
            // - Debe "contener rejilla": densidad de píxeles blancos (bin=255) intermedia
            const double GRID_ASPECT = 10.0 / 8.0; // ≈1.25
            const double ASPECT_TOL = 0.25;       // ±25%
            int bestScore = int.MinValue;
            Rect? bestLocal = null;

            foreach (var cnt in contours)
            {
                double area = Cv2.ContourArea(cnt);
                if (area < 60000 || area > 350000) continue;

                var rect = Cv2.BoundingRect(cnt);
                if (rect.Width < 100 || rect.Height < 100) continue; // evita ruido

                double aspect = (double)rect.Height / rect.Width;
                if (Math.Abs(aspect - GRID_ASPECT) > GRID_ASPECT * ASPECT_TOL) continue;

                // “Densidad de rejilla”: en el ROI recortado (un poco contraído)
                var inner = new Rect(
                    rect.X + rect.Width / 20, rect.Y + rect.Height / 20,
                    Math.Max(1, rect.Width  - rect.Width  / 10),
                    Math.Max(1, rect.Height - rect.Height / 10)
                );
                inner = Clamp(inner, search.Size());

                using var roi = new Mat(search, inner);
                int nz = Cv2.CountNonZero(roi);
                int tot = roi.Rows * roi.Cols;

                // esperamos entre 2%–15% de píxeles marcados en la rejilla
                double density = (double)nz / Math.Max(1, tot);
                if (density < 0.02 || density > 0.15) continue;

                // Puntuación: cuanto más cercano al aspecto ideal y mayor área, mejor
                int score = (int)(1000 * (1.0 - Math.Abs(aspect - GRID_ASPECT)) + area / 100.0);
                if (score > bestScore)
                {
                    bestScore = score;
                    bestLocal = inner;
                }
            }

            if (bestLocal.HasValue)
            {
                var r = bestLocal.Value;
                // pequeño padding para no cortar bordes de óvalos
                int padX = Math.Max(2, (int)(r.Width * 0.02));
                int padY = Math.Max(2, (int)(r.Height * 0.02));
                var padded = new Rect(
                    Math.Max(0, r.X - padX),
                    Math.Max(0, r.Y - padY),
                    Math.Min(search.Cols - Math.Max(0, r.X - padX), r.Width  + 2 * padX),
                    Math.Min(search.Rows - Math.Max(0, r.Y - padY), r.Height + 2 * padY)
                );

                // devolver en coords de la página
                return new Rect(
                    searchRect.X + padded.X,
                    searchRect.Y + padded.Y,
                    padded.Width,
                    padded.Height
                );
            }

            return null; // caerá al ROI fijo
        }


        private static string? ExtractDni(Mat bin, Mat pageBgr) // ← añadimos pageBgr para dibujar
        {
            // 1) ROI base (auto con fallback a ROI_DNI fijo)
            var auto = TryAutoLocateDniRoi(bin);
            var r = auto ?? ToRect(bin, ROI_DNI);

            // --- A) Recorte inicial por proporción 8×10 (alto/ancho ≈ 1.25) ---
            const double GRID_ASPECT = 10.0 / 8.0; // 1.25
            const double OUTER_PAD = 0.03;       // (afinado)
            int desiredH = (int)Math.Round(r.Width * GRID_ASPECT);
            int paddedH = (int)Math.Round(desiredH * (1.0 + OUTER_PAD));
            paddedH      = Math.Max(1, Math.Min(paddedH, bin.Rows - r.Y));
            r            = new Rect(r.X, r.Y, r.Width, paddedH);

            // --- B) Afinar alto con proyección vertical (10 filas) ---
            using (var prelim = new Mat(bin, r))
            {
                var py = new int[prelim.Rows];
                for (int y = 0; y < prelim.Rows; y++)
                {
                    using var row = new Mat(prelim, new Rect(0, y, prelim.Cols, 1));
                    py[y] = Cv2.CountNonZero(row);
                }

                var pySm = Smooth1D(py, Math.Max(3, prelim.Rows / 200 * 2 + 1));
                int minSepY = Math.Max(4, prelim.Rows / 16);
                var centersY0 = FindCenters1D(pySm, expectedCount: 10, minSep: minSepY);

                if (centersY0.Length >= 2)
                {
                    int first = centersY0.First();
                    int last = centersY0.Last();
                    int pad = Math.Max(2, (last - first) / 12);

                    int newY = Math.Max(r.Y + first - pad, 0);
                    int newB = Math.Min(r.Y + last + pad, bin.Rows - 1);
                    int newH = Math.Max(1, newB - newY + 1);

                    if (newH > 5) r = new Rect(r.X, newY, r.Width, newH);
                }
            }

            using var dniFull = new Mat(bin, r);

            // 2) Suavizado leve para estabilizar conteos
            using var dni = new Mat();
            Cv2.MedianBlur(dniFull, dni, 3);

            // 3) Proyecciones para centros de columnas (8) y filas (10)
            var projX = new int[dni.Cols];
            for (int x = 0; x < dni.Cols; x++)
            {
                using var col = new Mat(dni, new Rect(x, 0, 1, dni.Rows));
                projX[x] = Cv2.CountNonZero(col);
            }
            var projY = new int[dni.Rows];
            for (int y = 0; y < dni.Rows; y++)
            {
                using var row = new Mat(dni, new Rect(0, y, dni.Cols, 1));
                projY[y] = Cv2.CountNonZero(row);
            }

            var smX = Smooth1D(projX, Math.Max(3, dni.Cols / 200 * 2 + 1));
            var smY = Smooth1D(projY, Math.Max(3, dni.Rows / 200 * 2 + 1));
            int minSepX = Math.Max(6, dni.Cols / 12);
            int minSepY2 = Math.Max(4, dni.Rows / 16);
            var centersX = FindCenters1D(smX, expectedCount: 8, minSep: minSepX);
            var centersY = FindCenters1D(smY, expectedCount: 10, minSep: minSepY2);

            // ==== DEBUG VISUAL ====
            try
            {
                string baseDir = @"D:\";
                Directory.CreateDirectory(baseDir);

                // 3.1 Página completa con ROI dibujado
                using (var pageCopy = pageBgr.Clone())
                {
                    // Dibujar ROI r en verde
                    Cv2.Rectangle(pageCopy, r, new Scalar(0, 255, 0), 3);

                    // también dibuja el ROI de búsqueda auto (si existió)
                    if (auto.HasValue)
                        Cv2.Rectangle(pageCopy, auto.Value, new Scalar(0, 200, 255), 2); // cian/ámbar

                    Cv2.ImWrite(Path.Combine(baseDir, "dni_page.png"), pageCopy);
                }

                // 3.2 Recorte DNI con centros
                using (var dniBgr = new Mat())
                {
                    Cv2.CvtColor(dni, dniBgr, ColorConversionCodes.GRAY2BGR);

                    // Si se detectaron centros, píntalos
                    if (centersX.Length == 8 && centersY.Length == 10)
                    {
                        foreach (var cx in centersX)
                            Cv2.Line(dniBgr, new Point(cx, 0), new Point(cx, dniBgr.Rows - 1), new Scalar(255, 0, 0), 1);
                        foreach (var cy in centersY)
                            Cv2.Line(dniBgr, new Point(0, cy), new Point(dniBgr.Cols - 1, cy), new Scalar(0, 0, 255), 1);

                        // puntos en intersecciones (centros)
                        foreach (var cx in centersX)
                            foreach (var cy in centersY)
                                Cv2.Circle(dniBgr, new Point(cx, cy), 6, new Scalar(0, 255, 255), -1);
                    }

                    Cv2.ImWrite(Path.Combine(baseDir, "dni_roi.png"), dniBgr);
                }
            }
            catch { /* ignora errores de escritura para no romper el flujo */ }
            // ==== /DEBUG VISUAL ====

            if (centersX.Length != 8 || centersY.Length != 10)
                return null; // no pudimos encajar rejilla con seguridad

            // 4) Para cada columna, elegir la fila con mayor "fill"
            var digits = new char[8];
            for (int c = 0; c < 8; c++)
            {
                int cx = centersX[c];
                int bestRow = -1;
                double bestF = 0.0;

                for (int rRow = 0; rRow < 10; rRow++)
                {
                    int cy = centersY[rRow];
                    int halfW = Math.Max(2, SpacingFromCenters(centersX, c)    / 2);
                    int halfH = Math.Max(2, SpacingFromCenters(centersY, rRow) / 2);

                    var win = new Rect(
                        Math.Max(0, cx - halfW),
                        Math.Max(0, cy - halfH),
                        Math.Min(dni.Cols - Math.Max(0, cx - halfW), 2 * halfW + 1),
                        Math.Min(dni.Rows - Math.Max(0, cy - halfH), 2 * halfH + 1)
                    );

                    using var cell = new Mat(dni, win);
                    double fill = CircularFillRatio(cell, 0.46);
                    if (fill > bestF) { bestF = fill; bestRow = rRow; }
                }

                if (bestRow < 0 || bestF < 0.12) return null;
                digits[c] = (char)('0' + bestRow);
            }

            string dniText = new string(digits);
            if (dniText.Length != 8 || !dniText.All(char.IsDigit)) return null;
            return dniText;

            // ===== helpers locales (idénticos a los tuyos) =====
            static int[] Smooth1D(int[] v, int win)
            {
                if (win < 3) win = 3;
                if ((win & 1) == 0) win++;
                int half = win >> 1;
                var outv = new int[v.Length];
                for (int i = 0; i < v.Length; i++)
                {
                    int left = Math.Max(0, i - half);
                    int right = Math.Min(v.Length - 1, i + half);
                    int sum = 0;
                    for (int j = left; j <= right; j++) sum += v[j];
                    outv[i] = sum / (right - left + 1);
                }
                return outv;
            }

            static int[] FindCenters1D(int[] sm, int expectedCount, int minSep)
            {
                double max = sm.Max();
                if (max <= 0) return Array.Empty<int>();
                double thr = max * 0.62; // ligero incremento para evitar ruido

                var candidates = new List<int>();
                for (int i = 1; i < sm.Length - 1; i++)
                    if (sm[i] > thr && sm[i] >= sm[i - 1] && sm[i] >= sm[i + 1]) candidates.Add(i);

                var peaks = candidates
                    .OrderByDescending(i => sm[i])
                    .Aggregate(new List<int>(), (acc, idx) =>
                    {
                        if (acc.All(j => Math.Abs(j - idx) >= minSep)) acc.Add(idx);
                        return acc;
                    })
                    .OrderBy(i => i)
                    .ToList();

                if (peaks.Count < expectedCount && peaks.Count >= 2)
                {
                    int first = peaks.First();
                    int last = peaks.Last();
                    double step = (last - first) / (double)(expectedCount - 1);
                    var fixedCenters = new int[expectedCount];
                    for (int k = 0; k < expectedCount; k++)
                        fixedCenters[k] = (int)Math.Round(first + k * step);
                    return fixedCenters;
                }

                if (peaks.Count > expectedCount)
                    peaks = peaks.OrderByDescending(i => sm[i]).Take(expectedCount).OrderBy(i => i).ToList();

                return peaks.ToArray();
            }

            static int SpacingFromCenters(int[] centers, int i)
            {
                if (centers.Length <= 1) return 10;
                if (i == 0) return Math.Max(10, centers[1] - centers[0]);
                if (i == centers.Length - 1) return Math.Max(10, centers[^1] - centers[^2]);
                return Math.Max(10, Math.Min(centers[i] - centers[i - 1], centers[i + 1] - centers[i]));
            }
        }



        private static List<OmrAnswer> ExtractAnswers(Mat bin)
        {
            var r = ToRect(bin, ROI_QA);
            using var qaMat = new Mat(bin, r);

            var results = new List<OmrAnswer>(COLS_QUEST * ROWS_PER_COL);

            // Dividimos ROI_QA en 4 columnas verticales iguales
            int colW = qaMat.Width / COLS_QUEST;
            int rowHPerCol = qaMat.Height / ROWS_PER_COL; // alto de cada fila dentro de cada columna

            for (int c = 0; c < COLS_QUEST; c++)
            {
                // Sub-ROI de la columna c
                var colRect = new Rect(c * colW, 0, colW, qaMat.Height);

                for (int rRow = 0; rRow < ROWS_PER_COL; rRow++)
                {
                    int questionNumber = c * ROWS_PER_COL + rRow + 1;

                    var rowRect = new Rect(colRect.X, rRow * rowHPerCol, colRect.Width, rowHPerCol);
                    using var rowMat = new Mat(qaMat, rowRect);

                    // Cada fila tiene 5 opciones A..E en horizontal
                    int optW = rowMat.Width / OPTIONS;

                    int bestCol = -1;
                    double bestFill = 0.0;
                    double[] fills = new double[OPTIONS];

                    for (int opt = 0; opt < OPTIONS; opt++)
                    {
                        var optRect = new Rect(opt * optW, 0, optW, rowMat.Height);
                        var inner = InnerRect(optRect, INNER_MARGIN);
                        inner = Clamp(inner, rowMat.Size());

                        using var optCell = new Mat(rowMat, inner);

                        // Máscara circular centrada (evita texto “1.” y bordes)
                        double fill = CircularFillRatio(optCell, CIRCLE_RADIUS);
                        fills[opt] = fill;

                        if (fill > bestFill)
                        {
                            bestFill = fill;
                            bestCol = opt;
                        }
                    }

                    // Elegimos SIEMPRE una salida por pregunta:
                    // - Si hay marca clara (umbral y gap), usamos A..E
                    // - Si no, devolvemos "X"
                    double second = fills.OrderByDescending(v => v).Skip(1).FirstOrDefault();
                    bool uniqueEnough = bestCol >= 0 &&
                                        bestFill >= MARK_MIN_FILL &&
                                        (bestFill - second) >= MARK_GAP_BEST;

                    string letter = uniqueEnough ? LETTERS[bestCol] : "X";
                    double? confidence = uniqueEnough ? Math.Clamp(bestFill, 0, 1) : null;

                    results.Add(new OmrAnswer
                    {
                        QuestionNumber = questionNumber,
                        SelectedLetter = letter,
                        Confidence = confidence
                    });
                }
            }

            return results;
        }


        // =========================
        // Helpers geométricos / métricos
        // =========================

        private static double CircularFillRatio(Mat binCell, double radiusRel)
        {
            // binCell: imagen binaria (0/255), suponemos "marca" = 255 (BinaryInv)
            int h = binCell.Rows, w = binCell.Cols;
            if (h <= 0 || w <= 0) return 0;

            // Centro
            int cx = w / 2;
            int cy = h / 2;

            // Radio relativo a la min(dim)
            int r = (int)(Math.Min(w, h) * radiusRel);
            r = Math.Max(1, r);

            // Creamos máscara circular
            using var mask = new Mat(h, w, MatType.CV_8UC1, Scalar.Black);
            Cv2.Circle(mask, new Point(cx, cy), r, Scalar.White, -1, LineTypes.AntiAlias);

            using var masked = new Mat();
            Cv2.BitwiseAnd(binCell, mask, masked);

            int marked = Cv2.CountNonZero(masked);
            int area = Cv2.CountNonZero(mask); // píxeles dentro del círculo

            if (area <= 0) return 0;
            return (double)marked / area;
        }

        private static Rect InnerRect(Rect r, double marginRatio)
        {
            int mx = (int)(r.Width * marginRatio);
            int my = (int)(r.Height * marginRatio);
            return new Rect(r.X + mx, r.Y + my, Math.Max(1, r.Width - 2 * mx), Math.Max(1, r.Height - 2 * my));
        }

        private static Rect ToRect(Mat m, RectPercent rp)
        {
            int x = (int)(m.Cols * rp.X);
            int y = (int)(m.Rows * rp.Y);
            int w = (int)(m.Cols * rp.W);
            int h = (int)(m.Rows * rp.H);
            var r = new Rect(x, y, w, h);
            r.X = Math.Clamp(r.X, 0, m.Cols - 1);
            r.Y = Math.Clamp(r.Y, 0, m.Rows - 1);
            r.Width  = Math.Clamp(r.Width, 1, m.Cols - r.X);
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
