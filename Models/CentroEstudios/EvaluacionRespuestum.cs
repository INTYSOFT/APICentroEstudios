using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_intiSoft.Models.CentroEstudios;

[Table("evaluacion_respuesta", Schema = "academia")]
[Index("EvaluacionId", "Version", Name = "ix_er_eval_version")]
[Index("EvaluacionId", Name = "ix_er_evaluacion")]
[Index("EvaluacionId", "Version", "PreguntaOrden", Name = "ux_er_eval_version_pregunta", IsUnique = true)]
public partial class EvaluacionRespuestum
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("evaluacion_id")]
    public int EvaluacionId { get; set; }

    [Column("version")]
    public int Version { get; set; }

    [Column("pregunta_orden")]
    public int PreguntaOrden { get; set; }

    [Column("respuesta")]
    public string? Respuesta { get; set; }

    [Column("fuente")]
    public string Fuente { get; set; } = null!;

    [Column("confianza")]
    [Precision(5, 4)]
    public decimal? Confianza { get; set; }

    [Column("tiempo_marca_ms")]
    public int? TiempoMarcaMs { get; set; }

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

    //evaluacion_programada_id
    [Column("evaluacion_programada_id")]
    public int? EvaluacionProgramadaId { get; set; }

    //seccion_id
    [Column("seccion_id")]
    public int? SeccionId { get; set; }

    //alumno_id
    [Column("alumno_id")]
    public int? AlumnoId { get; set; }

    //dni_alumno
    [Column("dni_alumno")]
    public string? DniAlumno { get; set; }


    [ForeignKey("EvaluacionId")]
    [InverseProperty("EvaluacionRespuesta")]
    public virtual Evaluacion? Evaluacion { get; set; } = null!;

    //EvaluacionProgramadaId
    [ForeignKey("EvaluacionProgramadaId")]
    [InverseProperty(nameof(EvaluacionProgramadum.EvaluacionRespuesta))] // ← ✅ nombre exacto del otro lado
    public virtual EvaluacionProgramadum? EvaluacionProgramada { get; set; }

    //seccionId
    [ForeignKey("SeccionId")]
    [InverseProperty("EvaluacionRespuesta")]
    public virtual Seccion? Seccion { get; set; }




}
