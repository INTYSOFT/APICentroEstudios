using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.CentroEstudios;

[Table("evaluacion", Schema = "academia")]
[Index("EvaluacionProgramadaId", "AlumnoId", Name = "ux_eval_ep_alumno", IsUnique = true)]
public partial class Evaluacion
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("evaluacion_programada_id")]
    public int EvaluacionProgramadaId { get; set; }

    [Column("alumno_id")]
    public int AlumnoId { get; set; }

    [Column("evaluacion_forma_id")]
    public int? EvaluacionFormaId { get; set; }

    [Column("seccion_snapshot_id")]
    public int? SeccionSnapshotId { get; set; }

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

    [ForeignKey("EvaluacionFormaId")]
    [InverseProperty("Evaluacions")]
    public virtual EvaluacionForma? EvaluacionForma { get; set; }

    [InverseProperty("Evaluacion")]
    public virtual EvaluacionNotum? EvaluacionNotum { get; set; }

    [ForeignKey("EvaluacionProgramadaId")]
    [InverseProperty("Evaluacions")]
    public virtual EvaluacionProgramadum EvaluacionProgramada { get; set; } = null!;

    [InverseProperty("Evaluacion")]
    public virtual ICollection<EvaluacionRespuestum> EvaluacionRespuesta { get; set; } = new List<EvaluacionRespuestum>();

    //alumno
    [ForeignKey("AlumnoId")]
    [InverseProperty("Evaluacions")]
    public virtual Alumno Alumno { get; set; } = null!;

    //carrera
    [ForeignKey("CarreraSnapshotId")]
    [InverseProperty("Evaluacions")]
    public virtual Carrera? CarreraSnapshot { get; set; }

    //seccion
    [ForeignKey("SeccionSnapshotId")]
    [InverseProperty("Evaluacions")]
    public virtual Seccion? SeccionSnapshot { get; set; }

    
}
