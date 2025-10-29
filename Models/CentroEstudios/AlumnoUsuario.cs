using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_intiSoft.Models.CentroEstudios;

[Table("alumno_usuario", Schema = "academia")]
[Index("AlumnoId", Name = "ux_au_alumno", IsUnique = true)]
[Index("UsuarioId", Name = "ux_au_usuario", IsUnique = true)]
public partial class AlumnoUsuario
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("alumno_id")]
    public int AlumnoId { get; set; }

    [Column("usuario_id")]
    public int UsuarioId { get; set; }

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

    [ForeignKey("AlumnoId")]
    [InverseProperty("AlumnoUsuario")]
    public virtual Alumno Alumno { get; set; } = null!;

    [ForeignKey("UsuarioId")]
    [InverseProperty("AlumnoUsuario")]
    public virtual Usuario Usuario { get; set; } = null!;
}
