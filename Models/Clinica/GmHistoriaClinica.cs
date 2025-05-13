using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Clinica;

[Table("gm_historia_clinica")]
[Index("PacienteId", Name = "gm_historia_clinica_paciente_id_key", IsUnique = true)]
public partial class GmHistoriaClinica
{
    [Key]
    [Column("historia_clinica_id")]
    public int HistoriaClinicaId { get; set; }

    [Column("paciente_id")]
    public int PacienteId { get; set; }

    [Column("fecha_apertura", TypeName = "timestamp without time zone")]
    public DateTime? FechaApertura { get; set; }

    [Column("observaciones")]
    public string? Observaciones { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [InverseProperty("HistoriaClinica")]
    public virtual ICollection<GmHistoriaClinicaEvento> GmHistoriaClinicaEventos { get; set; } = new List<GmHistoriaClinicaEvento>();
}
