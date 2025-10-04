using api_intiSoft.Models.Common;

namespace api_intiSoft.Dto.Logistica.Producto
{
    public class LgProductoVarianteDto 
    {
        public int ProductoVarianteId { get; set; }
        public int ProductoId { get; set; }
        public int? ListaFichaTecnicaId { get; set; }
        public string? Descripcion { get; set; }
        public bool? GestionarPrecio { get; set; }
        public bool? Activo { get; set; } = true;
        public string? Nombre { get; set; }
        public decimal? PrecioAumentoPorcentaje { get; set; }
        public decimal? PrecioAumento { get; set; }
        public decimal? PrecioFijo { get; set; }
        public ICollection<LgProductoVarianteDetalleDto> VarianteDetalles { get; set; } = new HashSet<LgProductoVarianteDetalleDto>();
    }
}
