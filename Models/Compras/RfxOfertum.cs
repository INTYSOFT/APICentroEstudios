using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Compras;

/// <summary>
/// Ofertas de proveedores a un RFx (precio total, plazos, adjuntos).
/// </summary>
[Table("rfx_oferta", Schema = "compras")]
[Index("TenantId", "RfxId", "ProveedorId", Name = "ix_rfx_oferta_prov")]
[Index("TenantId", "RfxId", "ProveedorId", Name = "uq_oferta", IsUnique = true)]
public partial class RfxOfertum
{
    [Key]
    [Column("rfx_oferta_id")]
    public Guid RfxOfertaId { get; set; }

    [Column("tenant_id")]
    public Guid TenantId { get; set; }

    [Column("rfx_id")]
    public Guid RfxId { get; set; }

    [Column("proveedor_id")]
    public Guid ProveedorId { get; set; }

    [Column("precio_total")]
    [Precision(18, 6)]
    public decimal PrecioTotal { get; set; }

    [Column("plazo_entrega_dias")]
    public short? PlazoEntregaDias { get; set; }

    [Column("validez_oferta_dias")]
    public short? ValidezOfertaDias { get; set; }

    [Column("adjuntos_json", TypeName = "jsonb")]
    public string AdjuntosJson { get; set; } = null!;

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
    [InverseProperty("RfxOferta")]
    public virtual Proveedor Proveedor { get; set; } = null!;

    [ForeignKey("RfxId")]
    [InverseProperty("RfxOferta")]
    public virtual Rfx Rfx { get; set; } = null!;
}
