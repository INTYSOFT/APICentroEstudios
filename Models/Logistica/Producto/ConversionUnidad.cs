using api_intiSoft.Models.Common;

namespace api_intiSoft.Models.Logistica.Producto;

// ConversionUnidad.cs (modelo limpio)
public partial class ConversionUnidad : AuditableEntity
{
    public int ConversionId { get; set; }

    public int UnidadOrigenId { get; set; }
    public int UnidadDestinoId { get; set; }

    public decimal FactorConversion { get; set; }

    public virtual UnidadMedidum UnidadOrigen { get; set; } = null!;
    public virtual UnidadMedidum UnidadDestino { get; set; } = null!;
}
