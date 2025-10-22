using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using PdfiumViewer;

namespace api_intiSoft.Services.AnswerSheets;

public sealed class AnswerSheetProcessor : IAnswerSheetProcessor
{
    private const int RenderDpi = 300;
    private const int DigitsPerDni = 8;
    private const int DigitOptions = 10;
    private const int BlocksPerSheet = 5;
    private const int QuestionsPerBlock = 10;
    private const int OptionsPerQuestion = 5;
    private const double MinimumBubbleRatio = 0.18;
    private const double DniHorizontalPadding = 0.2;
    private const double DniVerticalPadding = 0.18;
    private const double AnswerHorizontalPadding = 0.12;
    private const double AnswerVerticalPadding = 0.18;

    private static readonly NormalizedRect DniRegion = new(0.045, 0.17, 0.19, 0.66);
    private static readonly NormalizedRect AnswersRegion = new(0.27, 0.11, 0.66, 0.78);

    private readonly ILogger<AnswerSheetProcessor> _logger;

    public AnswerSheetProcessor(ILogger<AnswerSheetProcessor> logger)
    {
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<StudentAnswerSheet>> ProcessAsync(Stream pdfStream, CancellationToken cancellationToken)
    {
        if (pdfStream is null)
        {
            throw new ArgumentNullException(nameof(pdfStream));
        }

        var ownsStream = false;
        MemoryStream workingStream;

        if (pdfStream is MemoryStream memoryStream)
        {
            workingStream = memoryStream;
            if (workingStream.CanSeek)
            {
                workingStream.Position = 0;
            }
        }
        else
        {
            workingStream = new MemoryStream();
            await pdfStream.CopyToAsync(workingStream, cancellationToken).ConfigureAwait(false);
            workingStream.Position = 0;
            ownsStream = true;
        }

        try
        {
            return ProcessInternal(workingStream, cancellationToken);
        }
        catch (AnswerSheetProcessingException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error procesando el archivo PDF de respuestas.");
            throw new AnswerSheetProcessingException("No se pudo procesar el archivo PDF especificado.", ex);
        }
        finally
        {
            if (ownsStream)
            {
                workingStream.Dispose();
            }
        }
    }

    private IReadOnlyCollection<StudentAnswerSheet> ProcessInternal(Stream pdfStream, CancellationToken cancellationToken)
    {
        if (pdfStream.CanSeek)
        {
            pdfStream.Position = 0;
        }

        using var document = PdfDocument.Load(pdfStream);
        if (document.PageCount == 0)
        {
            throw new AnswerSheetProcessingException("El archivo PDF no contiene páginas.");
        }

        var results = new List<StudentAnswerSheet>(document.PageCount);

        for (var pageIndex = 0; pageIndex < document.PageCount; pageIndex++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using var bitmap = document.Render(pageIndex, RenderDpi, RenderDpi, PdfRenderFlags.Annotations);
            using var pageMatAlpha = BitmapConverter.ToMat(bitmap);
            using var pageMat = PreparePage(pageMatAlpha);
            using var binary = Binarize(pageMat);

            var dni = ExtractDni(binary, pageMat.Size());
            if (dni.Length != DigitsPerDni)
            {
                throw new AnswerSheetProcessingException($"No se pudo determinar el DNI en la página {pageIndex + 1}.");
            }

            var answers = ExtractAnswers(binary, pageMat.Size());
            if (answers.Count == 0)
            {
                throw new AnswerSheetProcessingException($"No se pudo detectar ninguna respuesta en la página {pageIndex + 1}.");
            }

            results.Add(new StudentAnswerSheet
            {
                Dni = dni,
                Answers = answers
            });
        }

        return results;
    }

    private static Mat PreparePage(Mat matWithAlpha)
    {
        Mat working;
        if (matWithAlpha.Channels() == 4)
        {
            working = new Mat();
            Cv2.CvtColor(matWithAlpha, working, ColorConversionCodes.BGRA2BGR);
        }
        else
        {
            working = matWithAlpha.Clone();
        }

        if (working.Height > working.Width)
        {
            var rotated = new Mat();
            Cv2.Rotate(working, rotated, RotateFlags.Rotate90Counterclockwise);
            working.Dispose();
            working = rotated;
        }

        return working;
    }

    private static Mat Binarize(Mat source)
    {
        using var gray = new Mat();
        Cv2.CvtColor(source, gray, ColorConversionCodes.BGR2GRAY);

        using var blurred = new Mat();
        Cv2.GaussianBlur(gray, blurred, new Size(5, 5), 0);

        var binary = new Mat();
        Cv2.AdaptiveThreshold(blurred, binary, 255, AdaptiveThresholdTypes.GaussianC, ThresholdTypes.BinaryInv, 35, 5);
        Cv2.MedianBlur(binary, binary, 3);
        return binary;
    }

    private static string ExtractDni(Mat binary, Size pageSize)
    {
        var approxRect = DniRegion.Scale(pageSize);
        var roi = RefineRect(binary, approxRect, pageSize, 0.05);
        var columnWidth = roi.Width / (double)DigitsPerDni;
        var rowHeight = roi.Height / (double)DigitOptions;
        var digits = new char[DigitsPerDni];

        for (var column = 0; column < DigitsPerDni; column++)
        {
            var bestScore = double.MinValue;
            var bestDigit = 0;

            for (var row = 0; row < DigitOptions; row++)
            {
                var optionRect = CreateOptionRect(roi, pageSize, column, row, columnWidth, rowHeight, DniHorizontalPadding, DniVerticalPadding);
                var ratio = EvaluateFilledRatio(binary, optionRect);

                if (ratio > bestScore)
                {
                    bestScore = ratio;
                    bestDigit = row;
                }
            }

            if (bestScore < MinimumBubbleRatio)
            {
                return string.Empty;
            }

            digits[column] = (char)('0' + bestDigit);
        }

        return new string(digits);
    }

    private static IReadOnlyCollection<QuestionAnswer> ExtractAnswers(Mat binary, Size pageSize)
    {
        var approxRect = AnswersRegion.Scale(pageSize);
        var roi = RefineRect(binary, approxRect, pageSize, 0.02);
        var blockWidth = roi.Width / (double)BlocksPerSheet;
        var rowHeight = roi.Height / (double)QuestionsPerBlock;

        var answers = new List<QuestionAnswer>(BlocksPerSheet * QuestionsPerBlock);

        for (var block = 0; block < BlocksPerSheet; block++)
        {
            for (var row = 0; row < QuestionsPerBlock; row++)
            {
                var questionNumber = block * QuestionsPerBlock + row + 1;

                var cellRect = new Rect(
                    (int)Math.Round(roi.X + block * blockWidth),
                    (int)Math.Round(roi.Y + row * rowHeight),
                    (int)Math.Round(blockWidth),
                    (int)Math.Round(rowHeight));

                cellRect = KeepInside(cellRect, roi, pageSize);

                var detection = DetectOption(binary, cellRect, pageSize);
                answers.Add(new QuestionAnswer
                {
                    QuestionNumber = questionNumber,
                    Option = detection.Option,
                    Confidence = detection.Confidence
                });
            }
        }

        return answers;
    }

    private static OptionDetection DetectOption(Mat binary, Rect cellRect, Size pageSize)
    {
        var optionWidth = cellRect.Width / (double)OptionsPerQuestion;
        var bestIndex = -1;
        var bestScore = double.MinValue;
        var secondScore = double.MinValue;

        for (var index = 0; index < OptionsPerQuestion; index++)
        {
            var optionRect = new Rect(
                (int)Math.Round(cellRect.X + index * optionWidth + optionWidth * AnswerHorizontalPadding),
                (int)Math.Round(cellRect.Y + cellRect.Height * AnswerVerticalPadding),
                (int)Math.Round(optionWidth * (1 - 2 * AnswerHorizontalPadding)),
                (int)Math.Round(cellRect.Height * (1 - 2 * AnswerVerticalPadding)));

            optionRect = KeepInside(optionRect, cellRect, pageSize);
            var score = EvaluateFilledRatio(binary, optionRect);

            if (score > bestScore)
            {
                secondScore = bestScore;
                bestScore = score;
                bestIndex = index;
            }
            else if (score > secondScore)
            {
                secondScore = score;
            }
        }

        if (bestIndex < 0 || bestScore < MinimumBubbleRatio)
        {
            return new OptionDetection(string.Empty, 0);
        }

        var letter = (char)('A' + bestIndex);
        var confidence = Math.Clamp(bestScore - Math.Max(0, secondScore), 0, 1);
        return new OptionDetection(letter.ToString(), confidence);
    }

    private static Rect CreateOptionRect(Rect parent, Size pageSize, int column, int row, double columnWidth, double rowHeight, double paddingX, double paddingY)
    {
        var x = parent.X + column * columnWidth + columnWidth * paddingX;
        var y = parent.Y + row * rowHeight + rowHeight * paddingY;
        var width = columnWidth * (1 - 2 * paddingX);
        var height = rowHeight * (1 - 2 * paddingY);

        var rect = new Rect(
            (int)Math.Round(x),
            (int)Math.Round(y),
            Math.Max(1, (int)Math.Round(width)),
            Math.Max(1, (int)Math.Round(height)));

        return KeepInside(rect, parent, pageSize);
    }

    private static Rect KeepInside(Rect rect, Rect parent, Size bounds)
    {
        var normalizedParent = NormalizeRect(parent, bounds);
        var normalizedRect = NormalizeRect(rect, bounds);

        var normalizedRectRight = normalizedRect.X + normalizedRect.Width;
        var normalizedRectBottom = normalizedRect.Y + normalizedRect.Height;
        var normalizedParentRight = normalizedParent.X + normalizedParent.Width;
        var normalizedParentBottom = normalizedParent.Y + normalizedParent.Height;

        var x1 = Math.Max(normalizedRect.X, normalizedParent.X);
        var y1 = Math.Max(normalizedRect.Y, normalizedParent.Y);
        var x2 = Math.Min(normalizedRectRight, normalizedParentRight);
        var y2 = Math.Min(normalizedRectBottom, normalizedParentBottom);

        if (x2 <= x1)
        {
            x2 = x1 + 1;
        }

        if (y2 <= y1)
        {
            y2 = y1 + 1;
        }

        return new Rect(x1, y1, x2 - x1, y2 - y1);
    }

    private static Rect RefineRect(Mat binary, Rect approxRect, Size pageSize, double paddingFactor)
    {
        var normalized = NormalizeRect(approxRect, pageSize);
        using var roi = new Mat(binary, normalized);
        var points = Cv2.FindNonZero(roi);
        if (points == null || points.Length == 0)
        {
            return normalized;
        }

        var bounding = Cv2.BoundingRect(points);
        var refined = new Rect(normalized.X + bounding.X, normalized.Y + bounding.Y, bounding.Width, bounding.Height);
        return ExpandRect(refined, pageSize, paddingFactor);
    }

    private static Rect ExpandRect(Rect rect, Size bounds, double factor)
    {
        var paddingX = (int)Math.Round(rect.Width * factor);
        var paddingY = (int)Math.Round(rect.Height * factor);

        var x = Math.Max(0, rect.X - paddingX);
        var y = Math.Max(0, rect.Y - paddingY);
        var width = Math.Min(bounds.Width - x, rect.Width + paddingX * 2);
        var height = Math.Min(bounds.Height - y, rect.Height + paddingY * 2);

        width = Math.Max(1, width);
        height = Math.Max(1, height);

        return new Rect(x, y, width, height);
    }

    private static Rect NormalizeRect(Rect rect, Size bounds)
    {
        var maxX = Math.Max(0, bounds.Width - 1);
        var maxY = Math.Max(0, bounds.Height - 1);

        var x = Math.Clamp(rect.X, 0, maxX);
        var y = Math.Clamp(rect.Y, 0, maxY);

        var width = rect.Width;
        var height = rect.Height;

        if (width <= 0)
        {
            width = 1;
        }

        if (height <= 0)
        {
            height = 1;
        }

        var maxWidth = Math.Max(1, bounds.Width - x);
        var maxHeight = Math.Max(1, bounds.Height - y);

        width = Math.Clamp(width, 1, maxWidth);
        height = Math.Clamp(height, 1, maxHeight);

        return new Rect(x, y, width, height);
    }

    private static double EvaluateFilledRatio(Mat binary, Rect rect)
    {
        var normalized = NormalizeRect(rect, binary.Size());
        if (normalized.Width <= 0 || normalized.Height <= 0)
        {
            return 0;
        }

        using var roi = new Mat(binary, normalized);
        using var mask = Mat.Zeros(roi.Rows, roi.Cols, MatType.CV_8UC1);
        var radius = Math.Max(2, (int)(Math.Min(mask.Rows, mask.Cols) * 0.45));
        Cv2.Circle(mask, new Point(mask.Cols / 2, mask.Rows / 2), radius, Scalar.All(255), -1);

        using var masked = new Mat();
        Cv2.BitwiseAnd(roi, mask, masked);

        var filled = Cv2.CountNonZero(masked);
        var total = Cv2.CountNonZero(mask);
        if (total == 0)
        {
            return 0;
        }

        return Math.Clamp((double)filled / total, 0, 1);
    }

    private readonly record struct NormalizedRect(double X, double Y, double Width, double Height)
    {
        public Rect Scale(Size size)
        {
            var x = (int)Math.Round(X * size.Width);
            var y = (int)Math.Round(Y * size.Height);
            var width = Math.Max(1, (int)Math.Round(Width * size.Width));
            var height = Math.Max(1, (int)Math.Round(Height * size.Height));
            return new Rect(x, y, width, height);
        }
    }

    private readonly record struct OptionDetection(string Option, double Confidence);
}
