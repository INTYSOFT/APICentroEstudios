using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Clinica;

[Table("gm_horario_medico")]
public partial class GmHorarioMedico
{
    [Key]
    [Column("horario_medico_id")]
    public int HorarioMedicoId { get; set; }

    [Column("medico_id")]
    public int MedicoId { get; set; }

    [Column("horario_apertura_id")]
    public int HorarioAperturaId { get; set; }

    [Column("hora_inicio")]
    public TimeOnly HoraInicio { get; set; }

    [Column("hora_fin")]
    public TimeOnly HoraFin { get; set; }

    [Column("especialidad_id")]
    public int? EspecialidadId { get; set; }

    [Column("sala_id")]
    public int? SalaId { get; set; }

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [Column("usuario_registro_id")]
    public int? UsuarioRegistroId { get; set; }

    [Column("fecha_registro", TypeName = "timestamp without time zone")]
    public DateTime? FechaRegistro { get; set; }

    [Column("usuario_actualizacion_id")]
    public int? UsuarioActualizacionId { get; set; }

    [Column("fecha_actualizacion", TypeName = "timestamp without time zone")]
    public DateTime? FechaActualizacion { get; set; }

    [ForeignKey("HorarioAperturaId")]
    [InverseProperty("GmHorarioMedicos")]
    public virtual GmHorarioApertura? HorarioApertura { get; set; } = null!;
}
