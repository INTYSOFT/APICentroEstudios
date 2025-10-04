using System.Collections.Generic;
using api_intiSoft.Models.Common;

namespace api_intiSoft.Models.Logistica.Producto;

public partial class UnidadMedidum : AuditableEntity
{
    public int UnidadMedidaId { get; set; }
    public string Nombre { get; set; } = null!;
    public string Abreviatura { get; set; } = null!;
    public string? Descripcion { get; set; }
    public bool? Activo { get; set; }
    public int? TipoUnidadMedidaId { get; set; }

    // Navegaciones
    public virtual TipoUnidadMedidum? TipoUnidadMedida { get; set; }

    // Conversions 1:N (dos roles hacia ConversionUnidad)
    public virtual ICollection<ConversionUnidad> LgConversionUnidadUnidadDestinos { get; set; } = new List<ConversionUnidad>();
    public virtual ICollection<ConversionUnidad> LgConversionUnidadUnidadOrigens { get; set; } = new List<ConversionUnidad>();

    // (Opcional) Si deseas, puedes agregar aquí colecciones usadas en ProductoPresentacion:
    // public virtual ICollection<ProductoPresentacion> ProductoPresentacionesLongitud { get; set; } = new List<ProductoPresentacion>();
    // public virtual ICollection<ProductoPresentacion> ProductoPresentacionesPeso     { get; set; } = new List<ProductoPresentacion>();
    // public virtual ICollection<ProductoPresentacion> ProductoPresentacionesContenido{ get; set; } = new List<ProductoPresentacion>();
}
