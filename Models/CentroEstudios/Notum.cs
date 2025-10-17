using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.CentroEstudios;

[Table("nota", Schema = "academia")]
[Index("EvaluacionId", Name = "ux_nota_evaluacion", IsUnique = true)]
public partial class Notum
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("evaluacion_id")]
    public int EvaluacionId { get; set; }

    [Column("alumno_id")]
    public int AlumnoId { get; set; }

    [Column("simulacro_id")]
    public int SimulacroId { get; set; }

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

    [ForeignKey("AlumnoId")]
    [InverseProperty("Nota")]
    public virtual Alumno Alumno { get; set; } = null!;

    [ForeignKey("EvaluacionId")]
    [InverseProperty("Notum")]
    public virtual Evaluacion Evaluacion { get; set; } = null!;

    [ForeignKey("SimulacroId")]
    [InverseProperty("Nota")]
    public virtual Simulacro Simulacro { get; set; } = null!;
}
