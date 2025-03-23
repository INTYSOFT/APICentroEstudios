using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_conversion_unidad")]
public partial class LgConversionUnidad
{
    [Key]
    [Column("conversion_id")]
    public int ConversionId { get; set; }

    [Column("unidad_origen_id")]
    public int UnidadOrigenId { get; set; }

    [Column("unidad_destino_id")]
    public int UnidadDestinoId { get; set; }

    [Column("factor_conversion")]
    [Precision(18, 6)]
    public decimal FactorConversion { get; set; }

    [ForeignKey("UnidadDestinoId")]
    [InverseProperty("LgConversionUnidadUnidadDestinos")]
    public virtual LgUnidadMedidum UnidadDestino { get; set; } = null!;

    [ForeignKey("UnidadOrigenId")]
    [InverseProperty("LgConversionUnidadUnidadOrigens")]
    public virtual LgUnidadMedidum UnidadOrigen { get; set; } = null!;
}
