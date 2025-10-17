using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.CentroEstudios;

[Table("evaluacion_clave", Schema = "academia")]
[Index("EvaluacionFormaId", Name = "ix_ecla_ef")]
public partial class EvaluacionClave
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("evaluacion_forma_id")]
    public int EvaluacionFormaId { get; set; }

    [Column("pregunta_orden")]
    public int PreguntaOrden { get; set; }

    [Column("respuesta")]
    public string Respuesta { get; set; } = null!;

    [Column("ponderacion")]
    [Precision(6, 2)]
    public decimal? Ponderacion { get; set; }

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
    [InverseProperty("EvaluacionClaves")]
    public virtual EvaluacionForma EvaluacionForma { get; set; } = null!;
}
