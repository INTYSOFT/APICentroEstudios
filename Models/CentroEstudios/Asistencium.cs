using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_intiSoft.Models.CentroEstudios;

[Table("asistencia", Schema = "academia")]
[Index("Fecha", "CursoId", "AlumnoId", Name = "ux_asistencia_unique", IsUnique = true)]
public partial class Asistencium
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("fecha")]
    public DateOnly Fecha { get; set; }

    [Column("curso_id")]
    public int CursoId { get; set; }

    [Column("seccion_id")]
    public int SeccionId { get; set; }

    [Column("alumno_id")]
    public int AlumnoId { get; set; }

    [Column("tomado_por")]
    public int? TomadoPor { get; set; }

    [Column("observacion")]
    public string? Observacion { get; set; }

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
    [InverseProperty("Asistencia")]
    public virtual Alumno Alumno { get; set; } = null!;

    [ForeignKey("CursoId")]
    [InverseProperty("Asistencia")]
    public virtual Curso Curso { get; set; } = null!;

    [ForeignKey("SeccionId")]
    [InverseProperty("Asistencia")]
    public virtual Seccion Seccion { get; set; } = null!;

    [ForeignKey("TomadoPor")]
    [InverseProperty("Asistencia")]
    public virtual Usuario? TomadoPorNavigation { get; set; }
}
