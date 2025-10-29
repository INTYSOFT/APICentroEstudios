using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_intiSoft.Models.CentroEstudios;

[Table("evaluacion_programada", Schema = "academia")]
[Index("CarreraId", Name = "ix_ep_carrera")]
[Index("CicloId", Name = "ix_ep_ciclo")]
[Index("SedeId", Name = "ix_ep_sede")]
[Index("TipoEvaluacionId", Name = "ix_ep_tipo")]
public partial class EvaluacionProgramadum
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("sede_id")]
    public int SedeId { get; set; }

    [Column("ciclo_id")]
    public int? CicloId { get; set; }

    [Column("tipo_evaluacion_id")]
    public int TipoEvaluacionId { get; set; }

    //estado_id
    [Column("estado_id")]
    public int? EstadoId { get; set; }

    [Column("nombre")]
    public string Nombre { get; set; } = null!;

    [Column("fecha_inicio")]
    public DateOnly FechaInicio { get; set; }

    [Column("hora_inicio")]
    public TimeOnly HoraInicio { get; set; }

    [Column("hora_fin")]
    public TimeOnly HoraFin { get; set; }


    [Column("carrera_id")]
    public int? CarreraId { get; set; }


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

    [InverseProperty("EvaluacionProgramada")]
    public virtual ICollection<EvaluacionDetalle>? EvaluacionDetalles { get; set; } = new List<EvaluacionDetalle>();


    [InverseProperty("EvaluacionProgramada")]
    public virtual ICollection<EvaluacionNotum>? EvaluacionNota { get; set; } = new List<EvaluacionNotum>();

    [InverseProperty("EvaluacionProgramada")]
    public virtual ICollection<Evaluacion>? Evaluacions { get; set; } = new List<Evaluacion>();

    [ForeignKey("TipoEvaluacionId")]
    [InverseProperty("EvaluacionProgramada")]
    public virtual TipoEvaluacion? TipoEvaluacion { get; set; } = null!;

    //EvaluacionProgramadaSeccions
    [InverseProperty("EvaluacionProgramada")]
    public virtual ICollection<EvaluacionProgramadaSeccion>? EvaluacionProgramadaSeccions { get; set; } = new List<EvaluacionProgramadaSeccion>();

    //EstadoId
    [ForeignKey("EstadoId")]
    [InverseProperty("EvaluacionProgramadas")]
    public virtual EstadoEvaluacionProgramadum? Estado { get; set; } = null!;

    //evaluacion_respuesta
    [InverseProperty("EvaluacionProgramada")]
    public virtual ICollection<EvaluacionRespuestum>? EvaluacionRespuesta { get; set; } = new List<EvaluacionRespuestum>();
}
