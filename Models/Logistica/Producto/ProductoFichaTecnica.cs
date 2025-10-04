using System.Collections.Generic;
using api_intiSoft.Models.Common;

namespace api_intiSoft.Models.Logistica.Producto;

/// <summary>
/// Representa los datos técnicos asociados a un producto.
/// </summary>
public partial class ProductoFichaTecnica : AuditableEntity
{
    public int ProductoFichaTecnicaId { get; set; }

    public int ProductoId { get; set; }

    public int? FichaTecnicaId { get; set; }

    public string? Descripcion { get; set; }

    public bool Activo { get; set; }

    public int? CategoriaFichaTecnicaDetalleId { get; set; }

    public int? CategoriaFichaTecnicaId { get; set; }

    public string? Nombre { get; set; }

    public int? ListaFichaTecnicaId { get; set; }

    // ==== Navegaciones ====
    public virtual Producto Producto { get; set; } = null!;                 // requerido por FK no-null
    public virtual FichaTecnica? FichaTecnica { get; set; }                 // opcional
    public virtual CategoriaFichaTecnicaDetalle? CategoriaFichaTecnicaDetalle { get; set; } // opcional
    public virtual CategoriaFichaTecnica? CategoriaFichaTecnica { get; set; }               // opcional

    public virtual ICollection<ProductoFichaTecnicaDetalle> ProductoFichaTecnicaDetalles { get; set; }
        = new List<ProductoFichaTecnicaDetalle>();

    //ListaFichaTecnica
    public virtual ListaFichaTecnica? ListaFichaTecnica { get; set; }       // opcional

}
