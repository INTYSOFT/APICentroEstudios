using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Clinica;

[Table("gm_horario_apertura")]
[Index("Fecha", "HoraInicio", "HoraFin", "SucursalId", Name = "gm_horario_apertura_fecha_hora_inicio_hora_fin_sucursal_id_key", IsUnique = true)]
public partial class GmHorarioApertura
{
    [Key]
    [Column("horario_apertura_id")]
    public int HorarioAperturaId { get; set; }

    [Column("tipo_horario_id")]
    public short TipoHorarioId { get; set; }

    [Column("fecha")]
    public DateOnly? Fecha { get; set; }

    [Column("hora_inicio")]
    public TimeOnly HoraInicio { get; set; }

    [Column("hora_fin")]
    public TimeOnly HoraFin { get; set; }

    [Column("sucursal_id")]
    public int? SucursalId { get; set; }

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

    [InverseProperty("HorarioApertura")]
    public virtual ICollection<GmHorarioMedico> GmHorarioMedicos { get; set; } = new List<GmHorarioMedico>();

    [ForeignKey("TipoHorarioId")]
    [InverseProperty("GmHorarioAperturas")]
    public virtual GmTipoHorario? TipoHorario { get; set; } = null!;
}
