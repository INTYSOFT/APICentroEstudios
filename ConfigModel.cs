using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ContrlAcademico
{
    public class ConfigModel
    {
        [JsonPropertyName("dpi")]
        public int Dpi { get; set; }

        [JsonPropertyName("dniRegion")]
        public RegionModel DniRegion { get; set; } = new RegionModel();

        [JsonPropertyName("dniRegionNorm")]
        public NormRoiModel DniRegionNorm { get; set; } = new NormRoiModel();

        [JsonPropertyName("answersGrid")]
        public GridModel AnswersGrid { get; set; } = new GridModel();

        [JsonPropertyName("answersGridNorm")]
        public NormGridModel AnswersGridNorm { get; set; } = new NormGridModel();

        [JsonPropertyName("apiEndpoint")]
        public string ApiEndpoint { get; set; } = string.Empty;

        public static ConfigModel Load(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"No se encontró el config.json en '{path}'.");

            // Leer crudo (incluye // comentarios si los hubiera)
            var json = File.ReadAllText(path);

            var opts = new JsonSerializerOptions
            {
                ReadCommentHandling           = JsonCommentHandling.Skip,
                AllowTrailingCommas           = true,
                PropertyNameCaseInsensitive   = true
            };

            var cfg = JsonSerializer.Deserialize<ConfigModel>(json, opts)
                      ?? throw new InvalidOperationException("Error al deserializar ConfigModel desde JSON.");

            return cfg;
        }

    }

    public class RegionModel
    {
        [JsonPropertyName("x")]
        public int X { get; set; }

        [JsonPropertyName("y")]
        public int Y { get; set; }

        [JsonPropertyName("w")]
        public int W { get; set; }

        [JsonPropertyName("h")]
        public int H { get; set; }
    }

    public class GridModel
    {
        [JsonPropertyName("startX")]
        public int StartX { get; set; }

        [JsonPropertyName("startY")]
        public int StartY { get; set; }

        [JsonPropertyName("dx")]
        public int Dx { get; set; }

        [JsonPropertyName("dy")]
        public int Dy { get; set; }

        [JsonPropertyName("bubbleW")]
        public int BubbleW { get; set; }

        [JsonPropertyName("bubbleH")]
        public int BubbleH { get; set; }

        [JsonPropertyName("cols")]
        public int Cols { get; set; }

        [JsonPropertyName("rows")]
        public int Rows { get; set; }

        [JsonPropertyName("blockCount")]
        public int BlockCount { get; set; }

        [JsonPropertyName("blockSpacing")]
        public int BlockSpacing { get; set; }

        [JsonPropertyName("columnOffsets")]
        public double[] ColumnOffsets { get; set; } = Array.Empty<double>();

        [JsonPropertyName("rowOffsets")]
        public double[] RowOffsets { get; set; } = Array.Empty<double>();

        [JsonPropertyName("blockOffsets")]
        public double[] BlockOffsets { get; set; } = Array.Empty<double>();
    }

    public class NormGridModel
    {
        [JsonPropertyName("startX")]
        public double StartX { get; set; }

        [JsonPropertyName("startY")]
        public double StartY { get; set; }

        [JsonPropertyName("dx")]
        public double Dx { get; set; }

        [JsonPropertyName("dy")]
        public double Dy { get; set; }

        [JsonPropertyName("bubbleW")]
        public double BubbleW { get; set; }

        [JsonPropertyName("bubbleH")]
        public double BubbleH { get; set; }
    }

    public class DniRegionModel
    {


        [JsonPropertyName("x")]
        public int X { get; set; }
        [JsonPropertyName("y")]
        public int Y { get; set; }
        [JsonPropertyName("w")]
        public int W { get; set; }
        [JsonPropertyName("h")]
        public int H { get; set; }


    }

    public class NormRoiModel
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double W { get; set; }
        public double H { get; set; }
    }


}
