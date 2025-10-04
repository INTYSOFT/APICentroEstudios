using System.Collections.Generic;
using api_intiSoft.Models.Common;

namespace api_intiSoft.Models.Logistica.Producto;

public partial class FichaTecnica : AuditableEntity
{
    public int FichaTecnicaId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public bool Activo { get; set; }

    // ==== Navegaciones inversas ====
    public virtual ICollection<CategoriaFichaTecnica> CategoriaFichaTecnicas { get; set; }
        = new List<CategoriaFichaTecnica>();

    public virtual ICollection<ProductoFichaTecnica> ProductoFichaTecnicas { get; set; }
        = new List<ProductoFichaTecnica>();
}
