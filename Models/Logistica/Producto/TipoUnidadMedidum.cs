using System.Collections.Generic;
using api_intiSoft.Models.Common;

namespace api_intiSoft.Models.Logistica.Producto;

public partial class TipoUnidadMedidum : AuditableEntity
{
    public int TipoUnidadMedidaId { get; set; }
    public string Nombre { get; set; } = null!;
    public string? Descripcion { get; set; }

    // Relación 1:N con UnidadMedidum
    public virtual ICollection<UnidadMedidum> LgUnidadMedida { get; set; } = new List<UnidadMedidum>();
}
