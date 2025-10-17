using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

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

    [ForeignKey(nameof(EvaluacionId))]
    [InverseProperty(nameof(Evaluacion.EvaluacionRespuesta))]
    public virtual Evaluacion Evaluacion { get; set; } = null!;
}
