namespace api_intiSoft.Dto.Logistica.Producto
{
    public class OnlyProductoSearchResultDto
    {
        public long ProductoId { get; set; }
        public long CategoriaId { get; set; }
        public long? MarcaId { get; set; }
        public string Categoria { get; set; } = default!;
        public string? Marca { get; set; }
        
        public string? Modelo { get; set; }

        public string Producto { get; set; } = default!;
        public double TotalScore { get; set; }
    }
}
