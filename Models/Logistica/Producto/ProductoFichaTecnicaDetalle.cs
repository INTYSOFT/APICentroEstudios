using api_intiSoft.Models.Common;

namespace api_intiSoft.Models.Logistica.Producto;

public partial class ProductoFichaTecnicaDetalle : AuditableEntity
{
    public int ProductoFichaTecnicaDetalleId { get; set; }

    public int ProductoFichaTecnicaId { get; set; }

    public int? DetalleListaFichaTecnicaId { get; set; }

    public string Dato { get; set; } = null!;

    public string? Descripcion { get; set; }

    public bool Activo { get; set; }

    // ==== Navegaciones ====
    public virtual ProductoFichaTecnica ProductoFichaTecnica { get; set; } = null!;
    public virtual DetalleListaFichaTecnica? DetalleListaFichaTecnica { get; set; }
}
