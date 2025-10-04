namespace api_intiSoft.Dto.Logistica.Producto
{
    public class LgCategoriaConRutaDto
    {
        public int CategoriaId { get; set; }
        public int? CategoriaMainId { get; set; }
        public bool? IsEnd { get; set; }
        public string Categoria { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        public bool IsEditadoFichaTecnica { get; set; }
        public int? CategoriaIdFichaTecnica { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public string? Ruta { get; set; }
    }
}
