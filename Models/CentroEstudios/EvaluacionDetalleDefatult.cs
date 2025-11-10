using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.CentroEstudios;

[Table("evaluacion_detalle_default", Schema = "academia")]
public partial class EvaluacionDetalleDefatult
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("rango_inicio")]
    public int RangoInicio { get; set; }

    [Column("rango_fin")]
    public int RangoFin { get; set; }

    [Column("valor_buena"), Precision(6, 2)]
    public decimal ValorBuena { get; set; }

    [Column("valor_mala"), Precision(6, 2)]
    public decimal ValorMala { get; set; }

    [Column("valor_blanca"), Precision(6, 2)]
    public decimal ValorBlanca { get; set; }

    [Column("observacion")]
    public string? Observacion { get; set; }

    [Column("activo")]
    public bool Activo { get; set; }

    [Column("fecha_registro")]
    public DateTime? FechaRegistro { get; set; }

    [Column("fecha_actualizacion")]
    public DateTime? FechaActualizacion { get; set; }

    // OJO: aquí corrijo los typos a 'usuario_*'
    [Column("usuario_registro_id")]
    public int? UsuarioRegistroId { get; set; }

    [Column("usuario_actualizacion_id")]
    public int? UsuarioActualizacionId { get; set; }

    [Column("evaluacion_tipo_pregunta_id")]
    public int EvaluacionTipoPreguntaId { get; set; }
}
