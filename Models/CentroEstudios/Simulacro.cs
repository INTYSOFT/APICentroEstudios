using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.CentroEstudios;

[Table("simulacro", Schema = "academia")]
public partial class Simulacro
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("sede_id")]
    public int SedeId { get; set; }

    [Column("nombre")]
    public string Nombre { get; set; } = null!;

    [Column("rango_inicio")]
    public int RangoInicio { get; set; }

    [Column("rango_fin")]
    public int RangoFin { get; set; }

    [Column("tipo")]
    public string? Tipo { get; set; }

    [Column("valor_buena")]
    [Precision(6, 2)]
    public decimal ValorBuena { get; set; }

    [Column("valor_mala")]
    [Precision(6, 2)]
    public decimal ValorMala { get; set; }

    [Column("valor_blanca")]
    [Precision(6, 2)]
    public decimal ValorBlanca { get; set; }

    [Column("fecha_inicio")]
    public DateOnly FechaInicio { get; set; }

    [Column("hora_inicio")]
    public TimeOnly HoraInicio { get; set; }

    [Column("hora_fin")]
    public TimeOnly HoraFin { get; set; }

    [Column("nivel_id")]
    public int? NivelId { get; set; }

    [Column("carrera_id")]
    public int? CarreraId { get; set; }

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

    [ForeignKey("CarreraId")]
    [InverseProperty("Simulacros")]
    public virtual Carrera? Carrera { get; set; }

    [InverseProperty("Simulacro")]
    public virtual ICollection<Evaluacion> Evaluacions { get; set; } = new List<Evaluacion>();


    [InverseProperty("Simulacro")]
    public virtual ICollection<Notum> Nota { get; set; } = new List<Notum>();

    [ForeignKey("SedeId")]
    [InverseProperty("Simulacros")]
    public virtual Sede Sede { get; set; } = null!;
}
