using api_intiSoft.Models.Common;

namespace api_intiSoft.Models.Logistica.Producto;

public partial class ProductoPresentacion : AuditableEntity
{
    public int ProductoPresentacionId { get; set; }
    public int ProductoId { get; set; }

    public string? Nombre { get; set; }
    public string? Descripcion { get; set; }
    public bool? Activo { get; set; }

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

    public string? NombreCompuestoCompleto { get; set; }
    public string? NombreCompuestoCorto { get; set; }
    public int? NumeracionGrupo { get; set; }
    public string? NombreGrupo { get; set; }

    public int? TipoUsoProductoId { get; set; }
    public decimal? CantUnidad { get; set; }

    // ==== Navegaciones ====
    public virtual Producto Producto { get; set; } = null!;
    public virtual Empaque? Empaque { get; set; }

    // Usamos la misma entidad de unidades para long., peso y contenido
    public virtual UnidadMedidum? UnidadMedidaLongitud { get; set; }
    public virtual UnidadMedidum? UnidadMedidaPeso { get; set; }
    public virtual UnidadMedidum? UnidadMedidaContenido { get; set; }

    public virtual CategoriaFichaTecnicaDetalle? CategoriaFichaTecnicaDetalle { get; set; }

    // Autorrelación
    public virtual ProductoPresentacion? ProductoPresentacionIncluye { get; set; }
    public virtual ICollection<ProductoPresentacion> IncluidasEn { get; set; } = new List<ProductoPresentacion>();
}
