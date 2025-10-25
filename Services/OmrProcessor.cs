using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Drawing;
using System.Linq;
using System.Text;


namespace ContrlAcademico.Services
{


    public class OmrProcessor
    {
        public double FillThreshold => _fillThreshold;

        private readonly RegionModel _dniRegion;
        private readonly double _fillThreshold; // % de área para considerar marcado


        readonly GridModel _g;
        readonly Mat _mask;         // máscara elíptica de la burbuja
        readonly double _meanThreshold; // umbral sobre media de gris
        readonly double _deltaMin;      // separación mínima entre 1º y 2º
        double _lastThreshold = double.NaN;

        public double LastThreshold => _lastThreshold;

        public string ReadDni(Mat threshDni)
        {
            // threshDni es el recorte binarizado de la región DNI
            const int cols = 8, rows = 10;
            int cellW = threshDni.Width  / cols;
            int cellH = threshDni.Height / rows;
            double area = cellW * cellH;
            double minFill = area * _fillThreshold;
            var sb = new StringBuilder(8);

            for (int c = 0; c < cols; c++)
            {
                double maxCnt = 0;
                int bestRow = -1;
                for (int r = 0; r < rows; r++)
                {
                    var cellRect = new Rect(c * cellW, r * cellH, cellW, cellH);
                    using var cell = new Mat(threshDni, cellRect);
                    double cnt = Cv2.CountNonZero(cell);
                    if (cnt > maxCnt)
                    {
                        maxCnt = cnt;
                        bestRow = r;
                    }
                }
                sb.Append(bestRow < 0 || maxCnt < minFill
                          ? '-'
                          : (char)('0' + bestRow));
            }

            return sb.ToString();
        }





        public OmrProcessor(
            GridModel grid,
            RegionModel dniRegion,         // ← nuevo parámetro
             double fillThreshold = 0.5,

            double meanThreshold = 180,   // intensidad media > esto → “sin marca”
            double deltaMin = 30)    // si 2º media – 1º media < deltaMin → ambigüedad
        {
            _g = grid;
            _meanThreshold = meanThreshold;
            _deltaMin      = deltaMin;

            _dniRegion     = dniRegion ?? throw new ArgumentNullException(nameof(dniRegion));
            _fillThreshold = fillThreshold;


            // Creamos la máscara elíptica del tamaño exacto de la burbuja:
            _mask = new Mat(_g.BubbleH, _g.BubbleW, MatType.CV_8UC1, Scalar.All(0));
            Cv2.Ellipse(
                _mask,
                //new Point(_g.BubbleW/2, _g.BubbleH/2),
                new OpenCvSharp.Point(_g.BubbleW/2, _g.BubbleH/2),
                new OpenCvSharp.Size(_g.BubbleW/2, _g.BubbleH/2),
                angle: 0, startAngle: 0, endAngle: 360,
                color: Scalar.All(255),
                thickness: -1
            );
        }


        public char[] Process(Bitmap warpedBmp)
        {
            // 1) Convertir a Mat y llevar a gris+blur
            using var src = BitmapConverter.ToMat(warpedBmp);
            using var gray = new Mat();
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
            Cv2.GaussianBlur(gray, gray, new OpenCvSharp.Size(5, 5), 0);

            int rows = _g.Rows;
            int cols = _g.Cols;
            int blocks = _g.BlockCount;

            // Trabajamos en punto flotante para evitar acumulación de errores
            double space = _g.BlockSpacing;
            double dx = _g.Dx;
            double dy = _g.Dy;

            double halfW = _g.BubbleW / 2.0;
            double halfH = _g.BubbleH / 2.0;

            int totalQuestions = rows * blocks;
            char[] answers = new char[totalQuestions];
            Array.Fill(answers, '-');

            var bestMeans = new double[totalQuestions];
            var secondMeans = new double[totalQuestions];
            var bestOpts = new int[totalQuestions];

            int idx = 0;

            // 2) Recorremos cada bloque de preguntas
            for (int b = 0; b < blocks; b++)
            {
                double baseCenterX = _g.StartX + b * space;

                for (int r = 0; r < rows; r++, idx++)
                {
                    double centerY = _g.StartY + r * dy;

                    // 3) Para cada opción A–E calculamos la media dentro de la elipse
                    var stats = Enumerable.Range(0, cols)
                        .Select(c =>
                        {
                            double centerX = baseCenterX + c * dx;

                            int x = (int)Math.Round(centerX - halfW);
                            int y = (int)Math.Round(centerY - halfH);

                            // Clamp ROI
                            int x2 = Math.Clamp(x, 0, Math.Max(0, gray.Width - 1));
                            int y2 = Math.Clamp(y, 0, Math.Max(0, gray.Height - 1));
                            int w2 = Math.Min(_g.BubbleW, gray.Width  - x2);
                            int h2 = Math.Min(_g.BubbleH, gray.Height - y2);

                            if (w2 <= 0 || h2 <= 0)
                                return (opt: c, mean: 255.0);

                            // Extraemos ROI y recortamos la máscara al mismo tamaño
                            using var roi = new Mat(gray, new Rect(x2, y2, w2, h2));
                            using var maskROI = new Mat(_mask, new Rect(0, 0, w2, h2));
                            // Media ponderada
                            Scalar m = Cv2.Mean(roi, maskROI);
                            return (opt: c, mean: m.Val0);
                        })
                        .OrderBy(t => t.mean) // las más oscuras (menor mean) primero
                        .ToArray();

                    var best = stats[0];
                    var second = stats.Length > 1
                        ? stats[1]
                        : (opt: -1, mean: double.PositiveInfinity);

                    bestMeans[idx] = best.mean;
                    secondMeans[idx] = second.mean;
                    bestOpts[idx] = best.opt;
                }
            }

            double adaptiveThreshold = ComputeAdaptiveThreshold(bestMeans);
            double effectiveThreshold = ResolveThreshold(adaptiveThreshold);
            _lastThreshold = effectiveThreshold;

            const double secondaryMarkedMargin = 5.0;   // margen para considerar que otra burbuja también está marcada
            const double secondaryNearMargin   = 3.0;   // tolerancia alrededor del umbral para decidir ambigüedad

            for (int i = 0; i < answers.Length; i++)
            {
                double bestMean = bestMeans[i];
                double secondMean = secondMeans[i];

                bool bestMarked = bestMean <= effectiveThreshold;
                bool hasSecond = !double.IsInfinity(secondMean);

                bool secondLikelyMarked = hasSecond && secondMean <= effectiveThreshold - secondaryMarkedMargin;
                bool secondTooClose = hasSecond && (secondMean - bestMean) < _deltaMin;
                bool secondNearThreshold = hasSecond && secondMean <= effectiveThreshold + secondaryNearMargin;

                bool ambiguous = secondLikelyMarked || (secondTooClose && secondNearThreshold);

                if (bestMarked && !ambiguous)
                {
                    answers[i] = (char)('A' + bestOpts[i]);
                }
            }

            return answers;
        }

        private double ComputeAdaptiveThreshold(double[] bestMeans)
        {
            if (bestMeans.Length == 0)
                return double.NaN;

            var valid = bestMeans
                .Where(v => !double.IsNaN(v) && !double.IsInfinity(v))
                .ToArray();

            if (valid.Length < 5)
                return double.NaN;

            Array.Sort(valid);

            double min = valid[0];
            double max = valid[^1];
            if (max - min < 10)
                return double.NaN;

            int cluster = Math.Clamp(valid.Length / 6, 3, 30);

            double darkAvg = valid.Take(cluster).Average();
            double lightAvg = valid.Skip(valid.Length - cluster).Average();

            if (lightAvg - darkAvg < 8)
                return double.NaN;

            double threshold = darkAvg + (lightAvg - darkAvg) * 0.65; // sesgo hacia burbujas en blanco
            threshold = Math.Clamp(threshold, min + 2, max - 2);

            return threshold;
        }

        private double ResolveThreshold(double adaptive)
        {
            if (double.IsNaN(adaptive))
                return _meanThreshold;

            double lowerBound = Math.Max(120, _meanThreshold - 80);
            double upperBound = 250;

            double clamped = Math.Clamp(adaptive, lowerBound, upperBound);

            // Suavizamos cambios muy bruscos respecto al valor configurado
            if (Math.Abs(clamped - _meanThreshold) > 45)
            {
                clamped = (_meanThreshold + clamped) / 2.0;
            }

            return clamped;
        }
    }
}
