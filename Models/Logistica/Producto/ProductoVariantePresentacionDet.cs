using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace api_intiSoft.Models.Logistica.Producto;

public partial class ProductoVariantePresentacionDet
{
    public int ProductoVariantePresentacionDetId { get; set; }
    public int ProductoVariantePresentacionId { get; set; }
    public int ProductoVarianteDetalleId { get; set; }
    public bool? Activo { get; set; }

    // ⬇⬇⬇ claves: nullable + ignorar en JSON y validación
    [JsonIgnore, ValidateNever]
    public virtual ProductoVariantePresentacion? ProductoVariantePresentacion { get; set; }

    [JsonIgnore, ValidateNever]
    public virtual ProductoVarianteDetalle? ProductoVarianteDetalle { get; set; }
}
