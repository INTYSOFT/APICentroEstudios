using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Clinica;

[Table("gm_historia_clinica_evento")]
[Index("FechaEvento", Name = "idx_evento_fecha")]
[Index("HistoriaClinicaId", Name = "idx_evento_historia")]
public partial class GmHistoriaClinicaEvento
{
    [Key]
    [Column("evento_id")]
    public int EventoId { get; set; }

    [Column("historia_clinica_id")]
    public int HistoriaClinicaId { get; set; }

    [Column("tipo_evento_id")]
    public int TipoEventoId { get; set; }

    [Column("cita_id")]
    public int? CitaId { get; set; }

    [Column("medico_id")]
    public int? MedicoId { get; set; }

    [Column("fecha_evento", TypeName = "timestamp without time zone")]
    public DateTime FechaEvento { get; set; }

    [Column("motivo_consulta")]
    public string? MotivoConsulta { get; set; }

    [Column("diagnostico")]
    public string? Diagnostico { get; set; }

    [Column("tratamiento")]
    public string? Tratamiento { get; set; }

    [Column("recomendaciones")]
    public string? Recomendaciones { get; set; }

    [Column("resumen")]
    public string? Resumen { get; set; }

    [Column("detalle")]
    public string? Detalle { get; set; }

    [ForeignKey("CitaId")]
    [InverseProperty("GmHistoriaClinicaEventos")]
    public virtual GmCitaMedica? Cita { get; set; }

    [ForeignKey("HistoriaClinicaId")]
    [InverseProperty("GmHistoriaClinicaEventos")]
    public virtual GmHistoriaClinica HistoriaClinica { get; set; } = null!;

    [ForeignKey("MedicoId")]
    [InverseProperty("GmHistoriaClinicaEventos")]
    public virtual GmMedico? Medico { get; set; }

    [ForeignKey("TipoEventoId")]
    [InverseProperty("GmHistoriaClinicaEventos")]
    public virtual GmTipoEventoClinico TipoEvento { get; set; } = null!;
}
