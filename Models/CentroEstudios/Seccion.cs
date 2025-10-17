using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.CentroEstudios;

[Table("seccion", Schema = "academia")]
[Index("Nombre", Name = "ux_seccion_ciclo_nombre", IsUnique = true)]
public partial class Seccion
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nombre")]
    public string Nombre { get; set; } = null!;

    [Column("capacidad")]
    public int Capacidad { get; set; }

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

    [InverseProperty(nameof(AperturaSeccion.Seccion))]
    public virtual ICollection<AperturaSeccion> AperturaSeccions { get; set; } = new List<AperturaSeccion>();

    [InverseProperty(nameof(Asistencium.Seccion))]
    public virtual ICollection<Asistencium> Asistencia { get; set; } = new List<Asistencium>();

    [InverseProperty(nameof(Evaluacion.SeccionSnapshot))]
    public virtual ICollection<Evaluacion> Evaluacions { get; set; } = new List<Evaluacion>();

    [InverseProperty(nameof(Matricula.Seccion))]
    public virtual ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();

    [InverseProperty(nameof(EvaluacionProgramadum.Seccion))]
    public virtual ICollection<EvaluacionProgramadum> Programaciones { get; set; } = new List<EvaluacionProgramadum>();

    [InverseProperty(nameof(SeccionCiclo.Seccion))]
    public virtual ICollection<SeccionCiclo> SeccionCiclos { get; set; } = new List<SeccionCiclo>();
}
