using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.CentroEstudios;

[Table("usuario", Schema = "academia")]
[Index("Email", Name = "ux_usuario_email", IsUnique = true)]
public partial class Usuario
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("email")]
    public string Email { get; set; } = null!;

    [Column("password_hash")]
    public string PasswordHash { get; set; } = null!;

    [Column("nombres")]
    public string? Nombres { get; set; }

    [Column("apellidos")]
    public string? Apellidos { get; set; }

    [Column("sede_id")]
    public int? SedeId { get; set; }

    [Column("activo")]
    public bool Activo { get; set; }

    [Column("fecha_registro")]
    public DateTime? FechaRegistro { get; set; }

    [Column("fecha_actualizacion")]
    public DateTime? FechaActualizacion { get; set; }

    [Column("usuaraio_registro_id")]
    public int? UsuaraioRegistroId { get; set; }

    [Column("usuaraio_actualizacion_id")]
    public int? UsuaraioActualizacionId { get; set; }

    [ForeignKey(nameof(SedeId))]
    [InverseProperty(nameof(Sede.Usuarios))]
    public virtual Sede? Sede { get; set; }

    [InverseProperty(nameof(AlumnoUsuario.Usuario))]
    public virtual AlumnoUsuario? AlumnoUsuario { get; set; }

    [InverseProperty(nameof(Asistencium.TomadoPorNavigation))]
    public virtual ICollection<Asistencium> Asistencia { get; set; } = new List<Asistencium>();

    [InverseProperty(nameof(AuditLog.Usuario))]
    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    [InverseProperty(nameof(BecaAlumno.AsignadoPorNavigation))]
    public virtual ICollection<BecaAlumno> BecaAlumnos { get; set; } = new List<BecaAlumno>();

    [InverseProperty(nameof(DocenteUsuario.Usuario))]
    public virtual DocenteUsuario? DocenteUsuario { get; set; }
}
