using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_cliente")]
public partial class LgCliente
{
    [Key]
    [Column("cliente_id")]
    public int ClienteId { get; set; }

    [Column("tipo_cliente_id")]
    public short TipoClienteId { get; set; }

    [Column("documento_identidad_id")]
    [StringLength(3)]
    public string DocumentoIdentidadId { get; set; } = null!;

    [Column("numero_documento")]
    [StringLength(20)]
    public string NumeroDocumento { get; set; } = null!;

    [Column("razon_social")]
    [StringLength(255)]
    public string? RazonSocial { get; set; }

    [Column("nombres")]
    [StringLength(100)]
    public string? Nombres { get; set; }

    [Column("apellidos")]
    [StringLength(150)]
    public string? Apellidos { get; set; }

    [Column("direccion")]
    [StringLength(255)]
    public string? Direccion { get; set; }

    [Column("ubigeo_id")]
    [StringLength(6)]
    public string? UbigeoId { get; set; }

    [Column("telefono")]
    [StringLength(20)]
    public string? Telefono { get; set; }

    [Column("celular")]
    [StringLength(20)]
    public string? Celular { get; set; }

    [Column("email")]
    [StringLength(100)]
    public string? Email { get; set; }

    [Column("activo")]
    public bool Activo { get; set; } = true;

    [Column("fecha_registro")]
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    [Column("fecha_actualizacion")]
    public DateTime? FechaActualizacion { get; set; }
    //fecha_nacimiento
    [Column("fecha_nacimiento")]
    public DateTime? FechaNacimiento { get; set; }

    [Column("usuario_registro_id")]
    public int? UsuarioRegistroId { get; set; }

    [Column("usuario_actualizacion_id")]
    public int? UsuarioActualizacionId { get; set; }

    [ForeignKey("DocumentoIdentidadId")]
    public virtual LgDocumentoIdentidad? DocumentoIdentidad { get; set; }

    [InverseProperty("Cliente")]
    public virtual ICollection<LgContactoCliente> LgContactoClientes { get; set; } = new List<LgContactoCliente>();

    [InverseProperty("Cliente")]
    public virtual ICollection<LgVentum> LgVenta { get; set; } = new List<LgVentum>();
}
