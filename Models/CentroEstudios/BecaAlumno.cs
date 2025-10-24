using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_intiSoft.Models.CentroEstudios;

[Table("beca_alumno", Schema = "academia")]
public partial class BecaAlumno
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("beca_id")]
    public int BecaId { get; set; }

    [Column("alumno_id")]
    public int AlumnoId { get; set; }

    [Column("matricula_id")]
    public int? MatriculaId { get; set; }

    [Column("asignado_por")]
    public int? AsignadoPor { get; set; }

    [Column("motivo")]
    public string? Motivo { get; set; }

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
    [InverseProperty("BecaAlumnos")]
    public virtual Alumno Alumno { get; set; } = null!;

    [ForeignKey("AsignadoPor")]
    [InverseProperty("BecaAlumnos")]
    public virtual Usuario? AsignadoPorNavigation { get; set; }

    [ForeignKey("BecaId")]
    [InverseProperty("BecaAlumnos")]
    public virtual Beca Beca { get; set; } = null!;

    [ForeignKey("MatriculaId")]
    [InverseProperty("BecaAlumnos")]
    public virtual Matricula? Matricula { get; set; }
}
