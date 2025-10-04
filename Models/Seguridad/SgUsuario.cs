using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Seguridad;

[Table("sg_usuario")]
public partial class SgUsuario
{
    [Key]
    [Column("usuario_id")]
    public int UsuarioId { get; set; }

    [Column("nombre_usuario")]
    [StringLength(50)]
    public string NombreUsuario { get; set; } = null!;

    [Column("contrasena")]
    [StringLength(255)]
    public string Contrasena { get; set; } = null!;

    [Column("nombre")]
    [StringLength(64)]
    public string Nombre { get; set; } = null!;

    [Column("apellido")]
    [StringLength(64)]
    public string Apellido { get; set; } = null!;

    [Column("telefono")]
    [StringLength(50)]
    public string? Telefono { get; set; }

    [Column("email")]
    [StringLength(100)]
    public string Email { get; set; } = null!;

    [Column("ip_address_publica")]
    [StringLength(50)]
    public string? IpAddressPublica { get; set; }

    [Column("ip_address_privada")]
    [StringLength(50)]
    public string? IpAddressPrivada { get; set; }

    [Column("ip_address_mac")]
    [StringLength(50)]
    public string? IpAddressMac { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [Column("rol_id")]
    public int? RolId { get; set; }

    [Column("fecha_registro", TypeName = "timestamp without time zone")]
    public DateTime? FechaRegistro { get; set; }

    [Column("fecha_actualizacion", TypeName = "timestamp without time zone")]
    public DateTime? FechaActualizacion { get; set; }

    [Column("usuaraio_registro_id")]
    public int? UsuaraioRegistroId { get; set; }

    [Column("usuaraio_actualizacion_id")]
    public int? UsuaraioActualizacionId { get; set; }
}
