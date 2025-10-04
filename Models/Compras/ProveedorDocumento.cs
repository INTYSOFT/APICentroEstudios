using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Compras;

/// <summary>
/// Documentos y evidencias del proveedor (contratos, certificados, pólizas, XML CPE, etc.) con vigencias y metadatos.
/// </summary>
[Table("proveedor_documento", Schema = "compras")]
[Index("TenantId", "ProveedorId", Name = "ix_proveedor_documento_prov")]
public partial class ProveedorDocumento
{
    [Key]
    [Column("proveedor_documento_id")]
    public Guid ProveedorDocumentoId { get; set; }

    [Column("tenant_id")]
    public Guid TenantId { get; set; }

    [Column("proveedor_id")]
    public Guid ProveedorId { get; set; }

    [Column("nombre")]
    public string Nombre { get; set; } = null!;

    [Column("url")]
    public string? Url { get; set; }

    [Column("hash_sha256")]
    public string? HashSha256 { get; set; }

    [Column("fecha_emision")]
    public DateOnly? FechaEmision { get; set; }

    [Column("fecha_caducidad")]
    public DateOnly? FechaCaducidad { get; set; }

    [Column("requiere_vigencia")]
    public bool RequiereVigencia { get; set; }

    [Column("metadatos_json", TypeName = "jsonb")]
    public string MetadatosJson { get; set; } = null!;

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
    [InverseProperty("ProveedorDocumentos")]
    public virtual Proveedor Proveedor { get; set; } = null!;
}
