namespace api_intiSoft.Dto.Logistica.Producto
{
    public class LgProductoVarianteDetalleDto
    {
        public int ProductoVarianteDetalleId { get; set; }
        public int ProductoVarianteId { get; set; }
        public int? DetalleListaFichaTecnicaId { get; set; }
        public string? VarianteSku { get; set; }
        public string? CodigoInterno { get; set; }
        public string? CodigoBarras { get; set; }
        public string? CodigoQr { get; set; }
        public string? Dato { get; set; }
        public string? Descripcion { get; set; }
        public bool? Activo { get; set; } = true;
        public decimal? PrecioAumentoPorcentaje { get; set; }
        public decimal? PrecioAumento { get; set; }
        public decimal? PrecioFijo { get; set; }
    }
}
