using api_intiSoft.Models.Common;

namespace api_intiSoft.Models.Logistica.Producto;

public partial class ProductoVarianteDetalle : AuditableEntity
{
    public int ProductoVarianteDetalleId { get; set; }
    public int ProductoVarianteId { get; set; }
    public int? DetalleListaFichaTecnicaId { get; set; }

    public string? Dato { get; set; }   // máx 64
    public string? Descripcion { get; set; }   // máx 512
    public bool? Activo { get; set; }   // default true si quieres

    //ProductoVariantePresentacions
    //public virtual ICollection<ProductoVariantePresentacion> ProductoVariantePresentacions { get; set; }        = new List<ProductoVariantePresentacion>();

    // ==== Navegaciones ====
    public virtual ProductoVariante ProductoVariante { get; set; } = null!;
    public virtual DetalleListaFichaTecnica? DetalleListaFichaTecnica { get; set; }
}
