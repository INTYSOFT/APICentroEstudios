using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Compras;

/// <summary>
/// Sedes/direcciones del proveedor: facturación, despacho, planta, etc. Soporta múltiples sedes y una predeterminada por proveedor.
/// </summary>
[Table("proveedor_sede", Schema = "compras")]
[Index("TenantId", "ProveedorId", Name = "ix_proveedor_sede_prov")]
public partial class ProveedorSede
{
    [Key]
    [Column("proveedor_sede_id")]
    public Guid ProveedorSedeId { get; set; }

    [Column("tenant_id")]
    public Guid TenantId { get; set; }

    [Column("proveedor_id")]
    public Guid ProveedorId { get; set; }

    [Column("es_predeterminada")]
    public bool EsPredeterminada { get; set; }

    [Column("tipo_sede")]
    public string TipoSede { get; set; } = null!;

    [Column("direccion_linea1")]
    public string DireccionLinea1 { get; set; } = null!;

    [Column("direccion_linea2")]
    public string? DireccionLinea2 { get; set; }

    [Column("ubigeo")]
    public string? Ubigeo { get; set; }

    [Column("ciudad")]
    public string? Ciudad { get; set; }

    [Column("region")]
    public string? Region { get; set; }

    [Column("codigo_postal")]
    public string? CodigoPostal { get; set; }

    [Column("pais_iso2")]
    [StringLength(2)]
    public string PaisIso2 { get; set; } = null!;

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
    [InverseProperty("ProveedorSedes")]
    public virtual Proveedor Proveedor { get; set; } = null!;

    [InverseProperty("ProveedorSede")]
    public virtual ICollection<ProveedorContacto> ProveedorContactos { get; set; } = new List<ProveedorContacto>();
}
