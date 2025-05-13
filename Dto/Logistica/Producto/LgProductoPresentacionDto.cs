using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace api_intiSoft.Dto.Logistica.Producto
{
    public class LgProductoPresentacionDto
    {
        
        public int ProductoPresentacionId { get; set; }

        public int ProductoId { get; set; }

        public string? PresentacionSku { get; set; }

        public string? Nombre { get; set; }

        public string? Descripcion { get; set; }

        public bool? Activo { get; set; }

        public string? CodigoBarras { get; set; }

        public byte[]? CodigoQr { get; set; }

        public string? CodigoInterno { get; set; }

        public int? EmpaqueId { get; set; }

        public decimal? Largo { get; set; }

        public decimal? Ancho { get; set; }

        public decimal? Altura { get; set; }

        public int? UnidadMedidaLongitudId { get; set; }

        public decimal? Peso { get; set; }

        public int? UnidadMedidaPesoId { get; set; }

        public int? CategoriaFichaTecnicaDetalleId { get; set; }

        public bool IncluyePresentacion { get; set; }

        public int? ProductoPresentacionIdIncluye { get; set; }

        public int? UnidadMedidaContenidoId { get; set; }

        public decimal? Contenido { get; set; }

        public decimal? CantidadIncluyePresentacion { get; set; }

        public short? Orden { get; set; }

    }
}
