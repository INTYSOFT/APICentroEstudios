using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_intiSoft.Models.CentroEstudios;

[Table("matricula", Schema = "academia")]
[Index("AlumnoId", Name = "ix_matricula_alumno")]
[Index("CarreraId", Name = "ix_matricula_carrera")]
[Index("SeccionCicloId", Name = "ix_matricula_seccion_ciclo")]
public partial class Matricula
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("alumno_id")]
    public int AlumnoId { get; set; }

    [Column("seccion_ciclo_id")]
    public int SeccionCicloId { get; set; }


    //sedeId
    [Column("sede_id")]
    public int? SedeId { get; set; }

    //cicloId
    [Column("ciclo_id")]
    public int? CicloId { get; set; }

    //seccionId
    [Column("seccion_id")]
    public int? SeccionId { get; set; }


    [Column("carnet_url")]
    public string? CarnetUrl { get; set; }

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

    [Column("carrera_id")]
    public int? CarreraId { get; set; }

    [InverseProperty("Matricula")]
    public virtual ICollection<MatriculaItem>? MatriculaItems { get; set; } = new List<MatriculaItem>();

    [ForeignKey("SeccionCicloId")]
    [InverseProperty("Matriculas")]
    public virtual SeccionCiclo? SeccionCiclo { get; set; } = null!;


    //alumno
    [ForeignKey("AlumnoId")]
    [InverseProperty("Matriculas")]
    public virtual Alumno? Alumno { get; set; } = null!;

    //Carrera
    [ForeignKey("CarreraId")]
    [InverseProperty("Matriculas")]
    public virtual Carrera? Carrera { get; set; } = null!;

    //BecaAlumnos
    [InverseProperty("Matricula")]
    public virtual ICollection<BecaAlumno>? BecaAlumnos { get; set; } = new List<BecaAlumno>();

    //Ordens
    [InverseProperty("Matricula")]
    public virtual ICollection<Orden>? Ordens { get; set; } = new List<Orden>();

}
