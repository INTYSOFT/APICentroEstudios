namespace api_intiSoft.Dto.Logistica.Producto
{
    public class LgProductoVarianteDto
    {
        public int ProductoId { get; set; }
        public string? VarianteSku { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcon { get; set; }
        public string? CodigoInterno { get; set; }
        public string? CodigoBarras { get; set; }
        public string? CodigoQr { get; set; }
        public bool? GestionarPrecio { get; set; }
        public int? FichaTecnicaId { get; set; }
        public int? CategoriaFichaTecnicaId { get; set; }
        public int? CategoriaFichaTecnicaDetalleId { get; set; }
        public int? CategoriaFichaTecnicaDetalleValorId { get; set; }
        public bool? Activo { get; set; }
        public int? ProductoPresentacionId { get; set; }
        public string? Variante { get; set; }
        public int? ListaFichaTecnicaId { get; set; }
        public int? DetalleListaFichaTecnicaId { get; set; }
    }
}
