using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_intiSoft.Models.CentroEstudios;

[Table("evaluacion_clave", Schema = "academia")]
[Index("CicloId", Name = "ix_ecla_ciclo")]
[Index("EvaluacionDetalleId", Name = "ix_ecla_ed")]
[Index("EvaluacionProgramadaId", Name = "ix_ecla_ep")]
[Index("PreguntaOrden", Name = "ix_ecla_pregunta")]
[Index("SeccionId", Name = "ix_ecla_seccion")]
[Index("SedeId", Name = "ix_ecla_sede")]
public partial class EvaluacionClave
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("evaluacion_programada_id")]
    public int EvaluacionProgramadaId { get; set; }

    [Column("evaluacion_detalle_id")]
    public int EvaluacionDetalleId { get; set; }

    [Column("pregunta_orden")]
    public int PreguntaOrden { get; set; }

    [Column("respuesta")]
    [StringLength(1)]
    public string Respuesta { get; set; } = null!;

    [Column("ponderacion")]
    [Precision(6, 2)]
    public decimal? Ponderacion { get; set; }

    [Column("version")]
    public int Version { get; set; }

    [Column("vigente")]
    public bool Vigente { get; set; }

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

    [Column("sede_id")]
    public int? SedeId { get; set; }

    [Column("ciclo_id")]
    public int? CicloId { get; set; }

    [Column("seccion_id")]
    public int? SeccionId { get; set; }
}
