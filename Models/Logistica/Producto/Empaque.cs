using api_intiSoft.Models.Common;

namespace api_intiSoft.Models.Logistica.Producto;

public partial class Empaque : AuditableEntity
{
    public int EmpaqueId { get; set; }

    public string Codigo { get; set; } = null!;   // ej. "BX", "PK", etc. (3 chars)
    public string Nombre { get; set; } = null!;   // ej. "Caja", "Paquete"
    public bool? Activo { get; set; }             // nullable para respetar tu esquema actual
}
