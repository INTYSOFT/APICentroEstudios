using System.Collections.Generic;
using api_intiSoft.Models.Common;

namespace api_intiSoft.Models.Logistica.Producto;

public partial class ProductoVariante : AuditableEntity, IAuditableWithLog
{
    public int ProductoVarianteId { get; set; }

    public int ProductoId { get; set; }
    public int? ListaFichaTecnicaId { get; set; }

    public string? Descripcion { get; set; }
    public bool? GestionarPrecio { get; set; }
    public bool? Activo { get; set; } = true;
    public string? Nombre { get; set; }

    // ==== Navegaciones ====
    public virtual Producto Producto { get; set; } = null!;
    public virtual ListaFichaTecnica? ListaFichaTecnica { get; set; }

    public virtual ICollection<ProductoVarianteDetalle> VarianteDetalles { get; set; }
        = new List<ProductoVarianteDetalle>();
    
}
