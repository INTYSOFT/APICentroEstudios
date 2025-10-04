using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Compras;

/// <summary>
/// Evaluaciones periódicas con KPIs y puntaje global (scorecards) por proveedor.
/// </summary>
[Table("proveedor_evaluacion", Schema = "compras")]
[Index("TenantId", "ProveedorId", Name = "ix_proveedor_evaluacion_prov")]
[Index("TenantId", "ProveedorId", "Periodo", Name = "uq_eval", IsUnique = true)]
public partial class ProveedorEvaluacion
{
    [Key]
    [Column("proveedor_evaluacion_id")]
    public Guid ProveedorEvaluacionId { get; set; }

    [Column("tenant_id")]
    public Guid TenantId { get; set; }

    [Column("proveedor_id")]
    public Guid ProveedorId { get; set; }

    [Column("periodo")]
    public string Periodo { get; set; } = null!;

    [Column("kpi_otd")]
    [Precision(5, 2)]
    public decimal? KpiOtd { get; set; }

    [Column("kpi_calidad_ppm")]
    [Precision(12, 2)]
    public decimal? KpiCalidadPpm { get; set; }

    [Column("kpi_costo_total")]
    [Precision(14, 2)]
    public decimal? KpiCostoTotal { get; set; }

    [Column("kpi_cumplimiento")]
    [Precision(5, 2)]
    public decimal? KpiCumplimiento { get; set; }

    [Column("kpi_esg")]
    [Precision(5, 2)]
    public decimal? KpiEsg { get; set; }

    [Column("puntaje_global")]
    [Precision(6, 2)]
    public decimal? PuntajeGlobal { get; set; }

    [Column("comentarios")]
    public string? Comentarios { get; set; }

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
    [InverseProperty("ProveedorEvaluacions")]
    public virtual Proveedor Proveedor { get; set; } = null!;
}
