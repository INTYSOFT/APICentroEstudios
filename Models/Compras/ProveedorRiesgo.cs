using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Compras;

/// <summary>
/// Riesgos del proveedor con severidad, exposición y plan/estado de mitigación.
/// </summary>
[Table("proveedor_riesgo", Schema = "compras")]
[Index("TenantId", "ProveedorId", Name = "ix_proveedor_riesgo_prov")]
public partial class ProveedorRiesgo
{
    [Key]
    [Column("proveedor_riesgo_id")]
    public Guid ProveedorRiesgoId { get; set; }

    [Column("tenant_id")]
    public Guid TenantId { get; set; }

    [Column("proveedor_id")]
    public Guid ProveedorId { get; set; }

    [Column("tipo_riesgo")]
    public string TipoRiesgo { get; set; } = null!;

    [Column("probabilidad_pct")]
    [Precision(5, 2)]
    public decimal? ProbabilidadPct { get; set; }

    [Column("impacto_pct")]
    [Precision(5, 2)]
    public decimal? ImpactoPct { get; set; }

    [Column("exposicion_pct")]
    [Precision(5, 2)]
    public decimal? ExposicionPct { get; set; }

    [Column("plan_mitigacion")]
    public string? PlanMitigacion { get; set; }

    [Column("fecha_objetivo_mitigacion")]
    public DateOnly? FechaObjetivoMitigacion { get; set; }

    [Column("estado_mitigacion")]
    public string? EstadoMitigacion { get; set; }

    [Column("activo")]
    public bool Activo { get; set; }

    [Column("fecha_registro")]
    public DateTime? FechaRegistro { get; set; }

    [Column("fecha_actualizacion")]
    public DateTime? FechaActualizacion { get; set; }

    [Column("usuaraio_registro_id")]
    public Guid? UsuaraioRegistroId { get; set; }

    [Column("usuaraio_actualizacion_id")]
    public Guid? UsuaraioActualizacionId { get; set; }

    [ForeignKey("ProveedorId")]
    [InverseProperty("ProveedorRiesgos")]
    public virtual Proveedor Proveedor { get; set; } = null!;
}
