using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Compras;

/// <summary>
/// Contactos (personas) del proveedor, opcionalmente vinculados a una sede específica.
/// </summary>
[Table("proveedor_contacto", Schema = "compras")]
[Index("TenantId", "ProveedorId", Name = "ix_proveedor_contacto_prov")]
public partial class ProveedorContacto
{
    [Key]
    [Column("proveedor_contacto_id")]
    public Guid ProveedorContactoId { get; set; }

    [Column("tenant_id")]
    public Guid TenantId { get; set; }

    [Column("proveedor_id")]
    public Guid ProveedorId { get; set; }

    [Column("proveedor_sede_id")]
    public Guid? ProveedorSedeId { get; set; }

    [Column("nombre")]
    public string Nombre { get; set; } = null!;

    [Column("cargo")]
    public string? Cargo { get; set; }

    [Column("telefono")]
    public string? Telefono { get; set; }

    [Column("email")]
    public string? Email { get; set; }

    [Column("area")]
    public string? Area { get; set; }

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
    [InverseProperty("ProveedorContactos")]
    public virtual Proveedor Proveedor { get; set; } = null!;

    [ForeignKey("ProveedorSedeId")]
    [InverseProperty("ProveedorContactos")]
    public virtual ProveedorSede? ProveedorSede { get; set; }
}
