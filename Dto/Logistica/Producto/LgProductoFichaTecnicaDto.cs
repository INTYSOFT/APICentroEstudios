namespace api_intiSoft.Dto.Logistica.Producto
{
    public class LgProductoFichaTecnicaDto
    {
        public int ProductoId { get; set; }
        public int? FichaTecnicaId { get; set; }
        public string Valor { get; set; } = null!;
        public string? Descripcion { get; set; }
        public bool Activo { get; set; }
        public int? CategoriaFichaTecnicaDetalleId { get; set; }
        public int? CategoriaFichaTecnicaDetalleValorId { get; set; }
        public int? CategoriaFichaTecnicaId { get; set; }
        public string? Nombre { get; set; }
        public int? ListaFichaTecnicaId { get; set; }
        public int? DetalleListaFichaTecnicaId { get; set; }
    }

}
