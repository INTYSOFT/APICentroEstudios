using System;
using System.Collections.Generic;
using System.Linq;
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
            public double FirstBubbleCenterX;
            public double FirstBubbleCenterY;
            public double[] ColumnOffsets = Array.Empty<double>();
            public double[] RowOffsets = Array.Empty<double>();
            public double[] BlockOffsets = Array.Empty<double>();
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

            var baseParams = new GridParams
            {
                Panel = inner,
                BlockCount = blocks,
                RowsPerBlock = rows,
                ColsPerQuestion = opts,
                BubbleW = bubbleW,
                BubbleH = bubbleH,
                DxRow = dyRow,
                DyOption = dxOpt,
                BlockSpacing = blockSp,
                FirstBubbleCenterX = inner.X + blockWidth * 0.19,
                FirstBubbleCenterY = inner.Y + dyRow * 0.16,
                ColumnOffsets = Enumerable.Range(0, opts).Select(i => i * dxOpt).ToArray(),
                RowOffsets = Enumerable.Range(0, rows).Select(i => i * dyRow).ToArray(),
                BlockOffsets = Enumerable.Range(0, blocks).Select(i => i * blockSp).ToArray()
            };

            return TryRefineGrid(gray, inner, baseParams) ?? baseParams;
        }

        private sealed class BubblePoint
        {
            public BubblePoint(double x, double y, double width, double height)
            {
                X = x;
                Y = y;
                Width = width;
                Height = height;
            }

            public double X { get; }
            public double Y { get; }
            public double Width { get; }
            public double Height { get; }
        }

        private sealed class BlockSummary
        {
            public BlockSummary(double[] rowCenters, double[][] columnCenters)
            {
                RowCenters = rowCenters;
                ColumnCenters = columnCenters;
                FirstColumnAverage = columnCenters.Average(row => row[0]);
            }

            public double[] RowCenters { get; }
            public double[][] ColumnCenters { get; }
            public double FirstColumnAverage { get; }
        }

        private GridParams? TryRefineGrid(MatCv gray, SD.Rectangle panel, GridParams seed)
        {
            var points = DetectBubblePoints(gray, panel, seed.BubbleW, seed.BubbleH);
            int expected = seed.BlockCount * seed.RowsPerBlock * seed.ColsPerQuestion;

            if (points.Count < expected * 0.6)
                return null;

            var blocks = SplitIntoBlocks(points, seed.BlockCount, seed.RowsPerBlock, seed.ColsPerQuestion);
            if (blocks.Count != seed.BlockCount || blocks.Any(b => b.Count == 0))
                return null;

            var blockSummaries = new List<BlockSummary>();
            foreach (var block in blocks)
            {
                if (block.Count < seed.RowsPerBlock * seed.ColsPerQuestion * 0.6)
                    return null;

                var rowGroups = ClusterByDimension(block, p => p.Y, seed.RowsPerBlock);
                if (rowGroups.Count != seed.RowsPerBlock || rowGroups.Any(g => g.Count == 0))
                    return null;

                var rowCenters = new double[seed.RowsPerBlock];
                var columnCenters = new double[seed.RowsPerBlock][];

                for (int r = 0; r < seed.RowsPerBlock; r++)
                {
                    var rowPoints = rowGroups[r];
                    rowCenters[r] = rowPoints.Average(p => p.Y);

                    var colGroups = ClusterByDimension(rowPoints, p => p.X, seed.ColsPerQuestion);
                    if (colGroups.Count != seed.ColsPerQuestion || colGroups.Any(g => g.Count == 0))
                        return null;

                    columnCenters[r] = new double[seed.ColsPerQuestion];
                    for (int c = 0; c < seed.ColsPerQuestion; c++)
                        columnCenters[r][c] = colGroups[c].Average(p => p.X);
                }

                blockSummaries.Add(new BlockSummary(rowCenters, columnCenters));
            }

            double startY = blockSummaries.Select(b => b.RowCenters[0]).Average();
            double[] rowOffsets = new double[seed.RowsPerBlock];
            for (int r = 0; r < seed.RowsPerBlock; r++)
            {
                var offsets = blockSummaries.Select(b => b.RowCenters[r] - b.RowCenters[0]).ToArray();
                rowOffsets[r] = offsets.Average();
            }
            rowOffsets[0] = 0;

            var firstColumnPerBlock = blockSummaries
                .Select(b => b.ColumnCenters.Average(row => row[0]))
                .ToArray();

            if (firstColumnPerBlock.Length == 0)
                return null;

            double startX = firstColumnPerBlock[0];
            double[] blockOffsets = new double[seed.BlockCount];
            for (int b = 0; b < seed.BlockCount; b++)
                blockOffsets[b] = firstColumnPerBlock[b] - startX;
            blockOffsets[0] = 0;

            double[] columnOffsets = new double[seed.ColsPerQuestion];
            for (int c = 0; c < seed.ColsPerQuestion; c++)
            {
                var offsets = new List<double>();
                foreach (var block in blockSummaries)
                {
                    foreach (var row in block.ColumnCenters)
                        offsets.Add(row[c] - row[0]);
                }

                columnOffsets[c] = offsets.Count == 0 ? 0 : offsets.Average();
            }
            columnOffsets[0] = 0;

            var dxSamples = new List<double>();
            var dySamples = new List<double>();
            var blockSamples = new List<double>();

            foreach (var block in blockSummaries)
            {
                for (int r = 0; r < seed.RowsPerBlock - 1; r++)
                    dySamples.Add(block.RowCenters[r + 1] - block.RowCenters[r]);

                for (int r = 0; r < seed.RowsPerBlock; r++)
                {
                    var row = block.ColumnCenters[r];
                    for (int c = 0; c < seed.ColsPerQuestion - 1; c++)
                        dxSamples.Add(row[c + 1] - row[c]);
                }
            }

            for (int b = 0; b < blockOffsets.Length - 1; b++)
                blockSamples.Add(blockOffsets[b + 1] - blockOffsets[b]);

            double dx = dxSamples.Count > 0 ? dxSamples.Average() : seed.DyOption;
            double dy = dySamples.Count > 0 ? dySamples.Average() : seed.DxRow;
            double blockSpacing = blockSamples.Count > 0 ? blockSamples.Average() : seed.BlockSpacing;

            if (dx <= 0 || dy <= 0 || blockSpacing <= 0)
                return null;

            for (int c = 1; c < columnOffsets.Length; c++)
            {
                if (columnOffsets[c] <= columnOffsets[c - 1])
                    columnOffsets[c] = columnOffsets[c - 1] + dx;
            }

            for (int r = 1; r < rowOffsets.Length; r++)
            {
                if (rowOffsets[r] <= rowOffsets[r - 1])
                    rowOffsets[r] = rowOffsets[r - 1] + dy;
            }

            for (int b = 1; b < blockOffsets.Length; b++)
            {
                if (blockOffsets[b] <= blockOffsets[b - 1])
                    blockOffsets[b] = blockOffsets[b - 1] + blockSpacing;
            }

            double bubbleW = Median(points.Select(p => p.Width)) * 1.05;
            double bubbleH = Median(points.Select(p => p.Height)) * 1.05;

            if (double.IsNaN(bubbleW) || double.IsNaN(bubbleH) || bubbleW <= 0 || bubbleH <= 0)
            {
                bubbleW = seed.BubbleW;
                bubbleH = seed.BubbleH;
            }

            columnOffsets[0] = 0;
            rowOffsets[0] = 0;
            if (blockOffsets.Length > 0)
                blockOffsets[0] = 0;

            return new GridParams
            {
                Panel = panel,
                BlockCount = seed.BlockCount,
                RowsPerBlock = seed.RowsPerBlock,
                ColsPerQuestion = seed.ColsPerQuestion,
                BubbleW = bubbleW,
                BubbleH = bubbleH,
                DxRow = dy,
                DyOption = dx,
                BlockSpacing = blockSpacing,
                FirstBubbleCenterX = startX,
                FirstBubbleCenterY = startY,
                ColumnOffsets = columnOffsets,
                RowOffsets = rowOffsets,
                BlockOffsets = blockOffsets
            };
        }

        private static List<BubblePoint> DetectBubblePoints(MatCv gray, SD.Rectangle panel, double approxW, double approxH)
        {
            var result = new List<BubblePoint>();
            if (panel.Width <= 0 || panel.Height <= 0)
                return result;

            using var roiGray = new MatCv(gray, new RectCv(panel.X, panel.Y, panel.Width, panel.Height));
            using var blur = new MatCv();
            Cv2.GaussianBlur(roiGray, blur, new SizeCv(5, 5), 0);

            using var thresh = new MatCv();
            Cv2.AdaptiveThreshold(blur, thresh, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.BinaryInv, 35, 7);

            using var kernel = Cv2.GetStructuringElement(MorphShapes.Ellipse, new SizeCv(3, 3));
            using var opened = new MatCv();
            Cv2.MorphologyEx(thresh, opened, MorphTypes.Open, kernel, iterations: 1);
            Cv2.MorphologyEx(opened, opened, MorphTypes.Close, kernel, iterations: 1);

            double approxArea = Math.Max(approxW * approxH, 1);
            double minArea = Math.Max(approxArea * 0.25, 40);
            double maxArea = approxArea * 4.0;

            var contours = Cv2.FindContoursAsArray(opened, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            foreach (var contour in contours)
            {
                double area = Cv2.ContourArea(contour);
                if (area < minArea || area > maxArea)
                    continue;

                var rect = Cv2.BoundingRect(contour);
                if (rect.Width <= 0 || rect.Height <= 0)
                    continue;

                double ratio = (double)rect.Width / rect.Height;
                if (ratio < 0.5 || ratio > 1.8)
                    continue;

                var m = Cv2.Moments(contour);
                if (Math.Abs(m.M00) < 1e-3)
                    continue;

                double cx = m.M10 / m.M00;
                double cy = m.M01 / m.M00;

                result.Add(new BubblePoint(panel.X + cx, panel.Y + cy, rect.Width, rect.Height));
            }

            return result;
        }

        private static List<List<BubblePoint>> SplitIntoBlocks(List<BubblePoint> points, int blockCount, int rows, int cols)
        {
            var sorted = points.OrderBy(p => p.X).ToList();
            var blocks = SplitByLargestGaps(sorted, blockCount);

            int expectedPerBlock = Math.Max(1, rows * cols);
            if (blocks.Count != blockCount || blocks.Any(b => b.Count < expectedPerBlock / 2))
            {
                blocks = SplitByCount(sorted, blockCount, expectedPerBlock);
            }

            return blocks.Count == blockCount ? blocks : new List<List<BubblePoint>>();
        }

        private static List<List<BubblePoint>> SplitByLargestGaps(List<BubblePoint> sorted, int blockCount)
        {
            if (blockCount <= 1 || sorted.Count == 0)
                return new List<List<BubblePoint>> { sorted };

            var gaps = new List<(double gap, int index)>();
            for (int i = 1; i < sorted.Count; i++)
                gaps.Add((sorted[i].X - sorted[i - 1].X, i));

            var splitIndices = gaps
                .OrderByDescending(g => g.gap)
                .Take(Math.Max(0, blockCount - 1))
                .Select(g => g.index)
                .OrderBy(i => i)
                .ToArray();

            var result = new List<List<BubblePoint>>();
            int start = 0;
            foreach (int idx in splitIndices)
            {
                result.Add(sorted.GetRange(start, idx - start));
                start = idx;
            }

            if (start < sorted.Count)
                result.Add(sorted.GetRange(start, sorted.Count - start));

            return result;
        }

        private static List<List<BubblePoint>> SplitByCount(List<BubblePoint> sorted, int blockCount, int expectedPerBlock)
        {
            var result = new List<List<BubblePoint>>();
            for (int b = 0; b < blockCount; b++)
            {
                int start = Math.Min(sorted.Count, b * expectedPerBlock);
                int end = b == blockCount - 1
                    ? sorted.Count
                    : Math.Min(sorted.Count, (b + 1) * expectedPerBlock);

                if (end <= start)
                {
                    result.Add(new List<BubblePoint>());
                    continue;
                }

                result.Add(sorted.GetRange(start, end - start));
            }

            return result;
        }

        private static List<List<BubblePoint>> ClusterByDimension(List<BubblePoint> points, Func<BubblePoint, double> selector, int clusters)
        {
            if (points.Count < clusters || clusters <= 0)
                return new List<List<BubblePoint>>();

            using var samples = new MatCv(points.Count, 1, MatType.CV_32F);
            for (int i = 0; i < points.Count; i++)
                samples.Set(i, 0, (float)selector(points[i]));

            using var labels = new MatCv();
            using var centers = new MatCv();
            var criteria = new TermCriteria(CriteriaTypes.Eps | CriteriaTypes.MaxIter, 40, 0.01);

            Cv2.Kmeans(samples, clusters, labels, criteria, 5, KMeansFlags.PpCenters, centers);

            var orderedCenters = new List<(double value, int index)>();
            for (int i = 0; i < clusters; i++)
                orderedCenters.Add((centers.At<float>(i, 0), i));

            orderedCenters.Sort((a, b) => a.value.CompareTo(b.value));

            var map = orderedCenters
                .Select((entry, idx) => (entry.index, idx))
                .ToDictionary(t => t.index, t => t.idx);

            var groups = new List<List<BubblePoint>>(Enumerable.Range(0, clusters).Select(_ => new List<BubblePoint>()));

            for (int i = 0; i < points.Count; i++)
            {
                int label = labels.At<int>(i, 0);
                int target = map[label];
                groups[target].Add(points[i]);
            }

            return groups;
        }

        private static double Median(IEnumerable<double> values)
        {
            var ordered = values.Where(v => !double.IsNaN(v) && !double.IsInfinity(v)).OrderBy(v => v).ToArray();
            if (ordered.Length == 0)
                return double.NaN;

            int mid = ordered.Length / 2;
            return ordered.Length % 2 == 0
                ? (ordered[mid - 1] + ordered[mid]) / 2.0
                : ordered[mid];
        }
    }
}
