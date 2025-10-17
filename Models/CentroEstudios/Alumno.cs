using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.CentroEstudios;

[Table("alumno", Schema = "academia")]
[Index("Dni", Name = "ux_alumno_dni", IsUnique = true)]
public partial class Alumno
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("dni")]
    [StringLength(12)]
    public string Dni { get; set; } = null!;

    [Column("apellidos")]
    public string? Apellidos { get; set; }

    [Column("nombres")]
    public string? Nombres { get; set; }

    [Column("fecha_nacimiento")]
    public DateOnly? FechaNacimiento { get; set; }

    [Column("celular")]
    public string? Celular { get; set; }

    [Column("correo")]
    public string? Correo { get; set; }

    [Column("ubigeo_code")]
    [StringLength(6)]
    public string? UbigeoCode { get; set; }

    //"colegio_id"
    [Column("colegio_id")]
    public int? ColegioId { get; set; }


    //direccion varchar(64)
    [Column("direccion")]
    public string? Direccion { get; set; }

    //observcaiones
    [Column("observacion")]
    public string? Observacion { get; set; }

    [Column("foto_url")]
    public string? FotoUrl { get; set; }

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

    [ForeignKey(nameof(ColegioId))]
    [InverseProperty(nameof(Colegio.Alumnos))]
    public virtual Colegio? Colegio { get; set; }

    [InverseProperty(nameof(AlumnoApoderado.Alumno))]
    public virtual ICollection<AlumnoApoderado> AlumnoApoderados { get; set; } = new List<AlumnoApoderado>();

    [InverseProperty(nameof(AlumnoUsuario.Alumno))]
    public virtual AlumnoUsuario? AlumnoUsuario { get; set; }

    [InverseProperty(nameof(Asistencium.Alumno))]
    public virtual ICollection<Asistencium> Asistencia { get; set; } = new List<Asistencium>();

    [InverseProperty(nameof(BecaAlumno.Alumno))]
    public virtual ICollection<BecaAlumno> BecaAlumnos { get; set; } = new List<BecaAlumno>();

    [InverseProperty(nameof(Evaluacion.Alumno))]
    public virtual ICollection<Evaluacion> Evaluacions { get; set; } = new List<Evaluacion>();

    [InverseProperty(nameof(EvaluacionNotum.Alumno))]
    public virtual ICollection<EvaluacionNotum> EvaluacionNota { get; set; } = new List<EvaluacionNotum>();

    [InverseProperty(nameof(Matricula.Alumno))]
    public virtual ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();

    [InverseProperty(nameof(Notum.Alumno))]
    public virtual ICollection<Notum> Nota { get; set; } = new List<Notum>();
}
