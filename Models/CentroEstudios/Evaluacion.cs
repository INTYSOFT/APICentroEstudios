using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.CentroEstudios;

[Table("evaluacion", Schema = "academia")]
[Index("SimulacroId", "AlumnoId", Name = "ux_evaluacion_simulacro_alumno", IsUnique = true)]
public partial class Evaluacion
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("simulacro_id")]
    public int SimulacroId { get; set; }

    [Column("alumno_id")]
    public int AlumnoId { get; set; }

    [Column("seccion_snapshot_id")]
    public int SeccionSnapshotId { get; set; }

    [Column("carrera_snapshot_id")]
    public int? CarreraSnapshotId { get; set; }

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
    [InverseProperty("Evaluacions")]
    public virtual Alumno Alumno { get; set; } = null!;

    [ForeignKey("CarreraSnapshotId")]
    [InverseProperty("Evaluacions")]
    public virtual Carrera? CarreraSnapshot { get; set; }

    [InverseProperty("Evaluacion")]
    public virtual Notum? Notum { get; set; }

    [ForeignKey("SeccionSnapshotId")]
    [InverseProperty("Evaluacions")]
    public virtual Seccion SeccionSnapshot { get; set; } = null!;

    [ForeignKey("SimulacroId")]
    [InverseProperty("Evaluacions")]
    public virtual Simulacro Simulacro { get; set; } = null!;
}
