using System.Collections.Generic;
using api_intiSoft.Models.Common;

namespace api_intiSoft.Models.Logistica.Producto;

public partial class ListaFichaTecnica : AuditableEntity
{
    public int ListaFichaTecnicaId { get; set; }

    public string Lista { get; set; } = null!;

    public string? Descripcion { get; set; }

    public bool Estado { get; set; }

    public short TipoListaId { get; set; }

    // Navegación inversa (1) Lista -> (N) Detalles (FK opcional en detalle)
    public virtual ICollection<DetalleListaFichaTecnica> DetalleListaFichaTecnicas { get; set; }
        = new List<DetalleListaFichaTecnica>();
}
