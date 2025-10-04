using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Compras;

/// <summary>
/// Procesos de sourcing (RFI/RFQ/RFP) con período de participación y moneda base.
/// </summary>
[Table("rfx", Schema = "compras")]
[Index("TenantId", "Codigo", Name = "ix_rfx_tenant_codigo")]
[Index("TenantId", "Codigo", Name = "uq_rfx_codigo", IsUnique = true)]
public partial class Rfx
{
    [Key]
    [Column("rfx_id")]
    public Guid RfxId { get; set; }

    [Column("tenant_id")]
    public Guid TenantId { get; set; }

    [Column("tipo")]
    public string Tipo { get; set; } = null!;

    [Column("codigo")]
    public string Codigo { get; set; } = null!;

    [Column("titulo")]
    public string Titulo { get; set; } = null!;

    [Column("fecha_apertura")]
    public DateTime FechaApertura { get; set; }

    [Column("fecha_cierre")]
    public DateTime FechaCierre { get; set; }

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

    [InverseProperty("Rfx")]
    public virtual ICollection<RfxItem> RfxItems { get; set; } = new List<RfxItem>();

    [InverseProperty("Rfx")]
    public virtual ICollection<RfxOfertum> RfxOferta { get; set; } = new List<RfxOfertum>();
}
