using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Compras;

/// <summary>
/// Maestro global de proveedores (multi-tenant, multi-país). Incluye identificadores fiscales, metadatos comerciales y estado del ciclo de vida.
/// </summary>
[Table("proveedor", Schema = "compras")]
public partial class Proveedor
{
    [Key]
    [Column("proveedor_id")]
    public Guid ProveedorId { get; set; }

    [Column("tenant_id")]
    public Guid TenantId { get; set; }

    [Column("razon_social")]
    public string RazonSocial { get; set; } = null!;

    [Column("nombre_comercial")]
    public string? NombreComercial { get; set; }

    [Column("nro_documento")]
    public string NroDocumento { get; set; } = null!;

    [Column("pais_iso2")]
    [StringLength(2)]
    public string PaisIso2 { get; set; } = null!;

    [Column("idioma_iso2")]
    [StringLength(2)]
    public string IdiomaIso2 { get; set; } = null!;

    [Column("telefono")]
    public string? Telefono { get; set; }

    [Column("email")]
    public string? Email { get; set; }

    [Column("sitio_web")]
    public string? SitioWeb { get; set; }

    [Column("es_agente_retencion")]
    public bool EsAgenteRetencion { get; set; }

    [Column("es_agente_percepcion")]
    public bool EsAgentePercepcion { get; set; }

    [Column("es_no_domiciliado")]
    public bool EsNoDomiciliado { get; set; }

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

    [InverseProperty("Proveedor")]
    public virtual ICollection<Contrato> Contratos { get; set; } = new List<Contrato>();

    [InverseProperty("Proveedor")]
    public virtual ICollection<ProveedorCategorium> ProveedorCategoria { get; set; } = new List<ProveedorCategorium>();

    [InverseProperty("Proveedor")]
    public virtual ICollection<ProveedorCertificacion> ProveedorCertificacions { get; set; } = new List<ProveedorCertificacion>();

    [InverseProperty("Proveedor")]
    public virtual ICollection<ProveedorCondicionPago> ProveedorCondicionPagos { get; set; } = new List<ProveedorCondicionPago>();

    [InverseProperty("Proveedor")]
    public virtual ICollection<ProveedorContacto> ProveedorContactos { get; set; } = new List<ProveedorContacto>();

    [InverseProperty("Proveedor")]
    public virtual ICollection<ProveedorCuentaBancarium> ProveedorCuentaBancaria { get; set; } = new List<ProveedorCuentaBancarium>();

    [InverseProperty("Proveedor")]
    public virtual ICollection<ProveedorDocumento> ProveedorDocumentos { get; set; } = new List<ProveedorDocumento>();

    [InverseProperty("Proveedor")]
    public virtual ICollection<ProveedorEvaluacion> ProveedorEvaluacions { get; set; } = new List<ProveedorEvaluacion>();

    [InverseProperty("Proveedor")]
    public virtual ICollection<ProveedorHomologacion> ProveedorHomologacions { get; set; } = new List<ProveedorHomologacion>();

    [InverseProperty("Proveedor")]
    public virtual ICollection<ProveedorPreferenciaLogistica> ProveedorPreferenciaLogisticas { get; set; } = new List<ProveedorPreferenciaLogistica>();

    [InverseProperty("Proveedor")]
    public virtual ICollection<ProveedorProducto> ProveedorProductos { get; set; } = new List<ProveedorProducto>();

    [InverseProperty("Proveedor")]
    public virtual ICollection<ProveedorRiesgo> ProveedorRiesgos { get; set; } = new List<ProveedorRiesgo>();

    [InverseProperty("Proveedor")]
    public virtual ICollection<ProveedorSede> ProveedorSedes { get; set; } = new List<ProveedorSede>();

    [InverseProperty("Proveedor")]
    public virtual ICollection<RfxOfertum> RfxOferta { get; set; } = new List<RfxOfertum>();
}
