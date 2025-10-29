using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

    [InverseProperty("Usuario")]
    public virtual AlumnoUsuario? AlumnoUsuario { get; set; }

    [InverseProperty("TomadoPorNavigation")]
    public virtual ICollection<Asistencium> Asistencia { get; set; } = new List<Asistencium>();

    [InverseProperty("Usuario")]
    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    [InverseProperty("AsignadoPorNavigation")]
    public virtual ICollection<BecaAlumno> BecaAlumnos { get; set; } = new List<BecaAlumno>();

    [InverseProperty("Usuario")]
    public virtual DocenteUsuario? DocenteUsuario { get; set; }
}
