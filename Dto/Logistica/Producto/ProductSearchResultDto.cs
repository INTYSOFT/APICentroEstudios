namespace api_intiSoft.Dto.Logistica.Producto
{
    public class ProductSearchResultDto
    {
        public string? NombreProducto { get; set; }
        public string? DescripcionProducto { get; set; }
        public string? Categoria { get; set; }
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public string? NombreVariante { get; set; }
        public string? NombrePresentacion { get; set; }
        public string? DescripcionPresentacion { get; set; }
        public int? Orden { get; set; }
        public string? NombreGrupo { get; set; }                // usa underscores: Dapper lo mapea
        public int? NumeracionGrupo { get; set; }
        public string? NombreCompuestoCompleto { get; set; }
        public string? NombreCompuestoCorto { get; set; }
        
        public string? Sku { get; set; }

        public string? CodigoBarra { get; set; }
        public string? CodigoQr { get; set; }
        public string? CodigoInterno { get; set; }
        
        public decimal? PrecioCompra { get; set; }
        public decimal? PrecioVentaFinal { get; set; }
        public decimal? Stock { get; set; }


        public int ProductoId { get; set; }
        public int? MarcaId { get; set; }
        public int? CategoriaId { get; set; }
        public int? ProductoPresentacionId { get; set; }
        public int? ProductoVariantePresentacionId { get; set; }
        public bool ActivoProducto { get; set; }
        public bool? ActivoPresentacion { get; set; }
        public bool? ActivoVariantePresentacion { get; set; }
        public decimal Score { get; set; }

        
    }
}
