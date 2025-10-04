namespace api_intiSoft.Dto.Logistica.Producto
{
    public class ProductSearchQuery
    {
        public string Q { get; set; } = default!;  // requerido
        public string? Exclude { get; set; }       // opcional: "azul blanco" o "\"verde lima\""
        public int Limit { get; set; } = 50;       // [1..200]
        public int Offset { get; set; } = 0;       // >= 0
        public bool NoFuzzy { get; set; } = false; // true = rendimiento máximo, solo FTS
        public string? TenantSchema { get; set; }  // opcional: "public" (default) u otro schema
    }
}
