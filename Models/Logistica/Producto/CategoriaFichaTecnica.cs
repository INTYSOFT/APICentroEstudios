using System.Collections.Generic;
using api_intiSoft.Models.Common;

namespace api_intiSoft.Models.Logistica.Producto;

public partial class CategoriaFichaTecnica : AuditableEntity
{
    public int CategoriaFichaTecnicaId { get; set; }

    public int? CategoriaId { get; set; }

    public int? FichaTecnicaId { get; set; }

    public bool? Activo { get; set; }

    // ==== Navegaciones ====
    public virtual Categorias? Categoria { get; set; }
    public virtual FichaTecnica? FichaTecnica { get; set; }

    public virtual ICollection<CategoriaFichaTecnicaDetalle?> CategoriaFichaTecnicaDetalles { get; set; }
        = new List<CategoriaFichaTecnicaDetalle>();
}
