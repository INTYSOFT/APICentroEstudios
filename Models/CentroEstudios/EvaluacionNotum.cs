using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_intiSoft.Models.CentroEstudios;

[Table("evaluacion_nota", Schema = "academia")]
[Index("Activo", Name = "ix_en_activo")]
[Index("AlumnoId", Name = "ix_en_alumno")]
[Index("EvaluacionProgramadaId", Name = "ix_en_ep")]
[Index("PublicadoEn", Name = "ix_en_publicado")]
[Index("EvaluacionId", Name = "ux_en_evaluacion", IsUnique = true)]
public partial class EvaluacionNotum
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("evaluacion_id")]
    public int EvaluacionId { get; set; }

    [Column("evaluacion_programada_id")]
    public int? EvaluacionProgramadaId { get; set; }

    [Column("alumno_id")]
    public int? AlumnoId { get; set; }

    [Column("buenas")]
    public int Buenas { get; set; }

    [Column("malas")]
    public int Malas { get; set; }

    [Column("blancas")]
    public int Blancas { get; set; }

    [Column("puntaje_bruto")]
    [Precision(8, 2)]
    public decimal PuntajeBruto { get; set; }

    [Column("puntaje_escalado")]
    [Precision(8, 2)]
    public decimal? PuntajeEscalado { get; set; }

    [Column("percentil")]
    [Precision(5, 2)]
    public decimal? Percentil { get; set; }

    [Column("ranking_seccion")]
    public int? RankingSeccion { get; set; }

    [Column("ranking_ciclo")]
    public int? RankingCiclo { get; set; }

    [Column("publicado_en")]
    public DateTime? PublicadoEn { get; set; }

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

    [ForeignKey("EvaluacionId")]
    [InverseProperty("EvaluacionNotum")]
    public virtual Evaluacion Evaluacion { get; set; } = null!;

    [ForeignKey("EvaluacionProgramadaId")]
    [InverseProperty("EvaluacionNota")]
    public virtual EvaluacionProgramadum? EvaluacionProgramada { get; set; }
}
