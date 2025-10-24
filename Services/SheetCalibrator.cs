using System;
using SD = System.Drawing;                 // ← alias para System.Drawing
using OpenCvSharp;                         // usaremos alias abajo para algunos tipos
using SizeCv = OpenCvSharp.Size;           // ← alias explícito
using RectCv = OpenCvSharp.Rect;           // ← alias explícito
using MatCv = OpenCvSharp.Mat;

namespace api_intiSoft.Services
{
    public sealed class SheetCalibrator
    {
        public sealed class GridParams
        {
            public SD.Rectangle Panel;
            public int BlockCount;        // 4
            public int RowsPerBlock;      // 25
            public int ColsPerQuestion;   // 5
            public double BubbleW;
            public double BubbleH;
            public double DxRow;          // vertical (entre filas)
            public double DyOption;       // horizontal (entre opciones A..E)
            public double BlockSpacing;   // horizontal (entre bloques de 25)
        }

        /// Detecta el panel que contiene las 4 columnas de 25 preguntas y devuelve parámetros de grilla.
        public GridParams Calibrate(SD.Bitmap page)
        {
            using var mat = OpenCvSharp.Extensions.BitmapConverter.ToMat(page);

            // A gris y binarizado "negro sobre blanco"
            using var gray = new MatCv();
            Cv2.CvtColor(mat, gray, ColorConversionCodes.BGR2GRAY);

            using var blur = new MatCv();
            Cv2.GaussianBlur(gray, blur, new SizeCv(5, 5), 0);                 // ← SizeCv

            using var bin = new MatCv();
            Cv2.AdaptiveThreshold(blur, bin, 255,
                AdaptiveThresholdTypes.MeanC,
                ThresholdTypes.BinaryInv, 35, 10);

            // Cerrar huecos para rescatar el rectángulo exterior
            using var kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new SizeCv(7, 7)); // ← SizeCv
            using var opened = new MatCv();
            Cv2.MorphologyEx(bin, opened, MorphTypes.Close, kernel, iterations: 1);

            // Contorno rectangular grande en la mitad derecha
            var contours = Cv2.FindContoursAsArray(opened, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            RectCv best = default;     // ← RectCv
            double bestScore = 0;

            foreach (var c in contours)
            {
                var r = Cv2.BoundingRect(c); // r es RectCv
                if (r.Width < mat.Width * 0.3 || r.Height < mat.Height * 0.4) continue;
                if (r.X < mat.Width * 0.25) continue;

                double area = r.Width * r.Height;
                double aspect = (double)r.Height / Math.Max(1, r.Width);
                double score = area * aspect;

                if (score > bestScore)
                {
                    best = r;
                    bestScore = score;
                }
            }

            if (bestScore == 0)
                throw new InvalidOperationException("No se pudo localizar el panel de respuestas.");

            // Convertir el RectCv a Rectangle de System.Drawing
            var bestRect = new SD.Rectangle(best.X, best.Y, best.Width, best.Height);

            // Margen interno
            int marginX = (int)(bestRect.Width  * 0.02);
            int marginY = (int)(bestRect.Height * 0.02);
            var inner = new SD.Rectangle(bestRect.X + marginX, bestRect.Y + marginY,
                                         bestRect.Width - 2 * marginX, bestRect.Height - 2 * marginY);

            int blocks = 4, rows = 25, opts = 5;

            double blockWidth = inner.Width / (double)blocks;
            double bubbleW = blockWidth * 0.12;
            double bubbleH = inner.Height / (rows * 1.25);
            double dyRow = inner.Height / (double)rows;          // vertical entre filas
            double dxOpt = (blockWidth * 0.72) / (opts - 1);     // horizontal entre opciones
            double blockSp = blockWidth;

            return new GridParams
            {
                Panel = inner,
                BlockCount = blocks,
                RowsPerBlock = rows,
                ColsPerQuestion = opts,
                BubbleW = bubbleW,
                BubbleH = bubbleH,
                DxRow = dyRow,
                DyOption = dxOpt,
                BlockSpacing = blockSp
            };
        }
    }
}
