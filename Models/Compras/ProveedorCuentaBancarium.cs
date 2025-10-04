using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Compras;

/// <summary>
/// Cuentas bancarias del proveedor (PEN/USD/etc.), con soporte a CCI/IBAN, SWIFT/BIC y cuenta de detracciones.
/// </summary>
[Table("proveedor_cuenta_bancaria", Schema = "compras")]
[Index("TenantId", "ProveedorId", Name = "ix_cta_bancaria_prov")]
public partial class ProveedorCuentaBancarium
{
    [Key]
    [Column("proveedor_cuenta_bancaria_id")]
    public Guid ProveedorCuentaBancariaId { get; set; }

    [Column("tenant_id")]
    public Guid TenantId { get; set; }

    [Column("proveedor_id")]
    public Guid ProveedorId { get; set; }

    [Column("banco_nombre")]
    public string BancoNombre { get; set; } = null!;

    [Column("numero_cuenta")]
    public string NumeroCuenta { get; set; } = null!;

    [Column("cci_iban")]
    public string? CciIban { get; set; }

    [Column("titular")]
    public string? Titular { get; set; }

    [Column("swift_bic")]
    public string? SwiftBic { get; set; }

    [Column("pais_iso2")]
    [StringLength(2)]
    public string? PaisIso2 { get; set; }

    [Column("validada_kyr")]
    public bool ValidadaKyr { get; set; }

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
    [InverseProperty("ProveedorCuentaBancaria")]
    public virtual Proveedor Proveedor { get; set; } = null!;
}
