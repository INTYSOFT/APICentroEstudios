using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Clinica;

[Table("gm_cita_medica")]
public partial class LgCliente
{
    [Key]
    [Column("cita_id")]
    public int CitaId { get; set; }

    [Column("cliente_id")]
    public int ClienteId { get; set; }

    [Column("medico_id")]
    public int MedicoId { get; set; }

    [Column("estado_cita_id")]
    public int EstadoCitaId { get; set; }

    [Column("fecha_cita")]
    public DateOnly FechaCita { get; set; }

    [Column("hora_inicio")]
    public TimeOnly HoraInicio { get; set; }

    [Column("hora_fin")]
    public TimeOnly? HoraFin { get; set; }

    [Column("motivo_consulta")]
    public string? MotivoConsulta { get; set; }

    [Column("observaciones")]
    public string? Observaciones { get; set; }

    [Column("duracion_minutos")]
    public int? DuracionMinutos { get; set; }

    [Column("fecha_registro", TypeName = "timestamp without time zone")]
    public DateTime? FechaRegistro { get; set; }

    [Column("usuario_registro_id")]
    public int? UsuarioRegistroId { get; set; }

    [Column("usuario_actualizacion_id")]
    public int? UsuarioActualizacionId { get; set; }

    [Column("fecha_actualizacion", TypeName = "timestamp without time zone")]
    public DateTime? FechaActualizacion { get; set; }

    //Relaciones
    [ForeignKey("EstadoCitaId")]
    [InverseProperty("GmCitaMedicas")]
    public virtual GmEstadoCitum? EstadoCita { get; set; } = null!;

    //CitaId GmHistoriaClinicaEventos
    [InverseProperty("Cita")]
    public virtual ICollection<GmHistoriaClinicaEvento> GmHistoriaClinicaEventos { get; set; } = new List<GmHistoriaClinicaEvento>();

    //MedicoId 
    [ForeignKey("MedicoId")]
    [InverseProperty("GmCitaMedicas")]
    public virtual GmMedico Medico { get; set; } = null!;
}
