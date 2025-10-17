using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.CentroEstudios;

[Table("nivel", Schema = "academia")]
[Index("Nombre", Name = "ux_nivel_nombre", IsUnique = true)]
public partial class Nivel
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nombre")]
    public string Nombre { get; set; } = null!;

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

    [InverseProperty(nameof(AperturaSeccion.Nivel))]
    public virtual ICollection<AperturaSeccion> AperturaSeccions { get; set; } = new List<AperturaSeccion>();

    [InverseProperty(nameof(EvaluacionProgramadum.Nivel))]
    public virtual ICollection<EvaluacionProgramadum> EvaluacionProgramada { get; set; } = new List<EvaluacionProgramadum>();

    [InverseProperty(nameof(SeccionCiclo.Nivel))]
    public virtual ICollection<SeccionCiclo> SeccionCiclos { get; set; } = new List<SeccionCiclo>();

    [InverseProperty(nameof(Simulacro.Nivel))]
    public virtual ICollection<Simulacro> Simulacros { get; set; } = new List<Simulacro>();
}
