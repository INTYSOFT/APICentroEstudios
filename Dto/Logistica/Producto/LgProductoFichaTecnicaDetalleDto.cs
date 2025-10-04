namespace api_intiSoft.Dto.Logistica.Producto
{
    public class LgProductoFichaTecnicaDetalleDto
    {
        public int ProductoFichaTecnicaDetalleId { get; set; }
        public int ProductoFichaTecnicaId { get; set; }
        public int? DetalleListaFichaTecnicaId { get; set; }
        public string Dato { get; set; } = null!;
        public string? Descripcion { get; set; }
        public bool Activo { get; set; }
     
    }
}
