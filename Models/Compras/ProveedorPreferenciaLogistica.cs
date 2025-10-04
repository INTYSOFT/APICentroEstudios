using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Compras;

/// <summary>
/// Preferencias logísticas e Incoterms por proveedor: tiempos, transportistas y tolerancias.
/// </summary>
[Table("proveedor_preferencia_logistica", Schema = "compras")]
[Index("TenantId", "ProveedorId", Name = "ix_preferencia_logistica_prov")]
public partial class ProveedorPreferenciaLogistica
{
    [Key]
    [Column("proveedor_preferencia_logistica_id")]
    public Guid ProveedorPreferenciaLogisticaId { get; set; }

    [Column("tenant_id")]
    public Guid TenantId { get; set; }

    [Column("proveedor_id")]
    public Guid ProveedorId { get; set; }

    [Column("lead_time_prom_dias")]
    public short? LeadTimePromDias { get; set; }

    [Column("dias_despacho")]
    public string? DiasDespacho { get; set; }

    [Column("transportista_preferido")]
    public string? TransportistaPreferido { get; set; }

    [Column("tolerancia_cantidad_pct")]
    [Precision(5, 2)]
    public decimal? ToleranciaCantidadPct { get; set; }

    [Column("tolerancia_tiempo_dias")]
    public short? ToleranciaTiempoDias { get; set; }

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
    [InverseProperty("ProveedorPreferenciaLogisticas")]
    public virtual Proveedor Proveedor { get; set; } = null!;
}
