using api_intiSoft.Models.Common;

namespace api_intiSoft.Models.Logistica.Producto;

public partial class TipoProductos : AuditableEntity
{
    public short TipoProductoId { get; set; }
    public string TipoProducto { get; set; } = null!;
    public string? Descripcion { get; set; }
    public bool Activo { get; set; }
}
