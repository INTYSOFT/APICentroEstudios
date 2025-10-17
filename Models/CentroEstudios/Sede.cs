using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.CentroEstudios;

[Table("sede", Schema = "academia")]
public partial class Sede
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nombre")]
    public string Nombre { get; set; } = null!;

    [Column("ubigeo_code")]
    [StringLength(6)]
    public string UbigeoCode { get; set; } = null!;

    [Column("direccion")]
    public string? Direccion { get; set; }

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
    

    [InverseProperty(nameof(AperturaCiclo.Sede))]
    public virtual ICollection<AperturaCiclo> AperturaCiclos { get; set; } = new List<AperturaCiclo>();

    [InverseProperty(nameof(EvaluacionProgramadum.Sede))]
    public virtual ICollection<EvaluacionProgramadum> EvaluacionProgramada { get; set; } = new List<EvaluacionProgramadum>();

    [InverseProperty(nameof(Matricula.Sede))]
    public virtual ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();

    [InverseProperty(nameof(Orden.Sede))]
    public virtual ICollection<Orden> Ordens { get; set; } = new List<Orden>();

    [InverseProperty(nameof(SeccionCiclo.Sede))]
    public virtual ICollection<SeccionCiclo> SeccionCiclos { get; set; } = new List<SeccionCiclo>();

    [InverseProperty(nameof(Simulacro.Sede))]
    public virtual ICollection<Simulacro> Simulacros { get; set; } = new List<Simulacro>();

    [InverseProperty(nameof(Usuario.Sede))]
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
