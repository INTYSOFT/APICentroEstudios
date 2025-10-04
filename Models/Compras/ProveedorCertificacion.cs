using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Compras;

/// <summary>
/// Certificaciones del proveedor (ISO, SGS, OEA, etc.) con organismo emisor y vigencias.
/// </summary>
[Table("proveedor_certificacion", Schema = "compras")]
[Index("TenantId", "ProveedorId", Name = "ix_proveedor_certificacion_prov")]
[Index("TenantId", "ProveedorId", "CertificacionCodigo", "NumeroCertificado", Name = "uq_cert", IsUnique = true)]
public partial class ProveedorCertificacion
{
    [Key]
    [Column("proveedor_certificacion_id")]
    public Guid ProveedorCertificacionId { get; set; }

    [Column("tenant_id")]
    public Guid TenantId { get; set; }

    [Column("proveedor_id")]
    public Guid ProveedorId { get; set; }

    [Column("certificacion_codigo")]
    public string CertificacionCodigo { get; set; } = null!;

    [Column("organismo_emisor")]
    public string? OrganismoEmisor { get; set; }

    [Column("numero_certificado")]
    public string? NumeroCertificado { get; set; }

    [Column("fecha_emision")]
    public DateOnly? FechaEmision { get; set; }

    [Column("fecha_caducidad")]
    public DateOnly? FechaCaducidad { get; set; }

    [Column("url_certificado")]
    public string? UrlCertificado { get; set; }

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
    [InverseProperty("ProveedorCertificacions")]
    public virtual Proveedor Proveedor { get; set; } = null!;
}
