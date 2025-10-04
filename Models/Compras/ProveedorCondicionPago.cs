using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Compras;

/// <summary>
/// Términos de pago por proveedor (NetX, descuentos por pronto pago, retenciones de garantía).
/// </summary>
[Table("proveedor_condicion_pago", Schema = "compras")]
[Index("TenantId", "ProveedorId", Name = "ix_proveedor_condicion_pago_prov")]
public partial class ProveedorCondicionPago
{
    [Key]
    [Column("proveedor_condicion_pago_id")]
    public Guid ProveedorCondicionPagoId { get; set; }

    [Column("tenant_id")]
    public Guid TenantId { get; set; }

    [Column("proveedor_id")]
    public Guid ProveedorId { get; set; }

    [Column("termino_pago")]
    public string TerminoPago { get; set; } = null!;

    [Column("dias_pago")]
    public short? DiasPago { get; set; }

    [Column("descuento_pronto_pago_pct")]
    [Precision(5, 2)]
    public decimal? DescuentoProntoPagoPct { get; set; }

    [Column("dias_pronto_pago")]
    public short? DiasProntoPago { get; set; }

    [Column("retencion_garantia_pct")]
    [Precision(5, 2)]
    public decimal? RetencionGarantiaPct { get; set; }

    [Column("notas")]
    public string? Notas { get; set; }

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
    [InverseProperty("ProveedorCondicionPagos")]
    public virtual Proveedor Proveedor { get; set; } = null!;
}
