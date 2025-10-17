using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

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

    [InverseProperty(nameof(BecaAlumno.Matricula))]
    public virtual ICollection<BecaAlumno> BecaAlumnos { get; set; } = new List<BecaAlumno>();

    [InverseProperty(nameof(MatriculaItem.Matricula))]
    public virtual ICollection<MatriculaItem> MatriculaItems { get; set; } = new List<MatriculaItem>();

    [InverseProperty(nameof(Orden.Matricula))]
    public virtual ICollection<Orden> Ordens { get; set; } = new List<Orden>();

    [ForeignKey(nameof(AlumnoId))]
    [InverseProperty(nameof(Alumno.Matriculas))]
    public virtual Alumno Alumno { get; set; } = null!;

    [ForeignKey(nameof(CarreraId))]
    [InverseProperty(nameof(Carrera.Matriculas))]
    public virtual Carrera? Carrera { get; set; }

    [ForeignKey(nameof(CicloId))]
    [InverseProperty(nameof(Ciclo.Matriculas))]
    public virtual Ciclo? Ciclo { get; set; }

    [ForeignKey(nameof(SeccionId))]
    [InverseProperty(nameof(Seccion.Matriculas))]
    public virtual Seccion? Seccion { get; set; }

    [ForeignKey(nameof(SeccionCicloId))]
    [InverseProperty(nameof(SeccionCiclo.Matriculas))]
    public virtual SeccionCiclo SeccionCiclo { get; set; } = null!;

    [ForeignKey(nameof(SedeId))]
    [InverseProperty(nameof(Sede.Matriculas))]
    public virtual Sede? Sede { get; set; }
}
