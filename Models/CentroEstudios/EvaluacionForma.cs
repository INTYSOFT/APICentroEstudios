using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.CentroEstudios;

[Table("evaluacion_forma", Schema = "academia")]
[Index("EvaluacionProgramadaId", Name = "ix_ef_ep")]
public partial class EvaluacionForma
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("evaluacion_programada_id")]
    public int EvaluacionProgramadaId { get; set; }

    [Column("codigo")]
    public string Codigo { get; set; } = null!;

    [Column("descripcion")]
    public string? Descripcion { get; set; }

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

    [InverseProperty(nameof(EvaluacionClave.EvaluacionForma))]
    public virtual ICollection<EvaluacionClave> EvaluacionClaves { get; set; } = new List<EvaluacionClave>();

    [InverseProperty(nameof(EvaluacionPreguntum.EvaluacionForma))]
    public virtual ICollection<EvaluacionPreguntum> EvaluacionPregunta { get; set; } = new List<EvaluacionPreguntum>();

    [ForeignKey(nameof(EvaluacionProgramadaId))]
    [InverseProperty(nameof(EvaluacionProgramadum.EvaluacionFormas))]
    public virtual EvaluacionProgramadum EvaluacionProgramada { get; set; } = null!;

    [InverseProperty(nameof(Evaluacion.EvaluacionForma))]
    public virtual ICollection<Evaluacion> Evaluacions { get; set; } = new List<Evaluacion>();
}
