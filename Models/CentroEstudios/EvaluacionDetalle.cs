using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_intiSoft.Models.CentroEstudios;

[Table("evaluacion_detalle", Schema = "academia")]
[Index("EvaluacionProgramadaId", Name = "ix_ed_ep")]
[Index("EvaluacionProgramadaId", "RangoInicio", "RangoFin", Name = "ux_ed_ep_rango", IsUnique = true)]
public partial class EvaluacionDetalle
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("evaluacion_programada_id")]
    public int EvaluacionProgramadaId { get; set; }

    //seccion_id
    [Column("seccion_id")]
    public int? SeccionId { get; set; }

    //evaluacion_tipo_pregunta_id
    [Column("evaluacion_tipo_pregunta_id")]
    public int? EvaluacionTipoPreguntaId { get; set; }



    [Column("rango_inicio")]
    public int RangoInicio { get; set; }

    [Column("rango_fin")]
    public int RangoFin { get; set; }

    [Column("valor_buena")]
    [Precision(6, 2)]
    public decimal ValorBuena { get; set; }

    [Column("valor_mala")]
    [Precision(6, 2)]
    public decimal ValorMala { get; set; }

    [Column("valor_blanca")]
    [Precision(6, 2)]
    public decimal ValorBlanca { get; set; }

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

    [ForeignKey("EvaluacionProgramadaId")]
    [InverseProperty("EvaluacionDetalles")]
    public virtual EvaluacionProgramadum? EvaluacionProgramada { get; set; } = null!;

    //SeccionId
    [ForeignKey("SeccionId")]
    [InverseProperty("EvaluacionDetalles")]
    public virtual Seccion? Seccion { get; set; }

    //evaluacion_tipo_pregunta_id
    [ForeignKey("EvaluacionTipoPreguntaId")]
    [InverseProperty("EvaluacionDetalles")]
    public virtual EvaluacionTipoPreguntum? EvaluacionTipoPregunta { get; set; }



}
