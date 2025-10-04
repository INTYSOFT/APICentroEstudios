using api_intiSoft.Models.Common;
using api_intiSoft.Models.Logistica.Producto;

namespace api_intiSoft.Models.Logistica.Producto;

public partial class DetalleListaFichaTecnica : AuditableEntity
{
    public int DetalleListaFichaTecnicaId { get; set; }
    public int ListaFichaTecnicaId { get; set; }
    public string ListaDetalle { get; set; } = null!;
    public string? Descripcion { get; set; }
    public bool Estado { get; set; }

    // 🔹 navegación hacia la CABECERA (Lista)
    public virtual ListaFichaTecnica? ListaFichaTecnica { get; set; }

    public virtual ICollection<ProductoFichaTecnicaDetalle> LgProductoFichaTecnicaDetalles { get; set; }
        = new List<ProductoFichaTecnicaDetalle>();

    public virtual ICollection<ProductoVarianteDetalle> LgProductoVarianteDetalles { get; set; }
        = new List<ProductoVarianteDetalle>();


}
