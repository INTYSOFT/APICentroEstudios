using System.Collections.Generic;
using api_intiSoft.Models.Common;

namespace api_intiSoft.Models.Logistica.Producto;

public partial class Producto : AuditableEntity
{
    public int ProductoId { get; set; }

    public int? TipoProductoId { get; set; }

    public int? CategoriaId { get; set; }

    public int? PlanContableId { get; set; }

    public string? CodigoSunat { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string? Modelo { get; set; }

    public int? MarcaId { get; set; }

    public short? ListaPrecioId { get; set; }

    public bool? Activo { get; set; }

    public int? ModeloId { get; set; }

    public bool? IsComprable { get; set; }

    public bool? IsVendible { get; set; }

    public bool? IsGasto { get; set; }

    // Navegaciones
    public virtual Categorias? Categoria { get; set; }
    public virtual ICollection<ProductoFichaTecnica> ProductoFichaTecnicas { get; set; } = new List<ProductoFichaTecnica>();

    public virtual ICollection<ProductoPresentacion> ProductoPresentaciones { get; set; } = new List<ProductoPresentacion>();
    //VariantePresentaciones
    public virtual ICollection<ProductoVariantePresentacion> ProductoVariantePresentaciones { get; set; } = new List<ProductoVariantePresentacion>();


}
