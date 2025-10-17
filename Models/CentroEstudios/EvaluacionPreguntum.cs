using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.CentroEstudios;

[Table("evaluacion_pregunta", Schema = "academia")]
[Index("EvaluacionFormaId", Name = "ix_epre_ef")]
[Index("EvaluacionFormaId", "PreguntaOrden", Name = "ux_epre_unique", IsUnique = true)]
public partial class EvaluacionPreguntum
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("evaluacion_forma_id")]
    public int EvaluacionFormaId { get; set; }

    [Column("pregunta_orden")]
    public int PreguntaOrden { get; set; }

    [Column("tipo")]
    public string Tipo { get; set; } = null!;

    [Column("puntaje_buena")]
    [Precision(6, 2)]
    public decimal PuntajeBuena { get; set; }

    [Column("penalizacion_mala")]
    [Precision(6, 2)]
    public decimal PenalizacionMala { get; set; }

    [Column("permite_blanca")]
    public bool PermiteBlanca { get; set; }

    [Column("meta", TypeName = "jsonb")]
    public string? Meta { get; set; }

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

    [ForeignKey(nameof(EvaluacionFormaId))]
    [InverseProperty(nameof(EvaluacionForma.EvaluacionPregunta))]
    public virtual EvaluacionForma EvaluacionForma { get; set; } = null!;
}
