using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.CentroEstudios;

[Table("seccion_ciclo", Schema = "academia")]
[Index("CicloId", Name = "ix_sc_ciclo")]
[Index("NivelId", Name = "ix_sc_nivel")]
[Index("SeccionId", Name = "ix_sc_seccion")]
[Index("SedeId", Name = "ix_sc_sede")]
public partial class SeccionCiclo
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("ciclo_id")]
    public int CicloId { get; set; }

    [Column("seccion_id")]
    public int SeccionId { get; set; }

    [Column("nivel_id")]
    public int NivelId { get; set; }

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

    [Column("capacidad")]
    public int? Capacidad { get; set; }

    [Column("sede_id")]
    public int SedeId { get; set; }

    [Column("precio")]
    [Precision(12, 2)]
    public decimal? Precio { get; set; }

    [InverseProperty("SeccionCiclo")]
    public virtual ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();

    [InverseProperty(nameof(EvaluacionProgramadum.SeccionCiclo))]
    public virtual ICollection<EvaluacionProgramadum> EvaluacionProgramada { get; set; } = new List<EvaluacionProgramadum>();

    [ForeignKey(nameof(CicloId))]
    [InverseProperty(nameof(Ciclo.SeccionCiclos))]
    public virtual Ciclo Ciclo { get; set; } = null!;

    [ForeignKey(nameof(NivelId))]
    [InverseProperty(nameof(Nivel.SeccionCiclos))]
    public virtual Nivel Nivel { get; set; } = null!;

    [ForeignKey(nameof(SeccionId))]
    [InverseProperty(nameof(Seccion.SeccionCiclos))]
    public virtual Seccion Seccion { get; set; } = null!;

    [ForeignKey(nameof(SedeId))]
    [InverseProperty(nameof(Sede.SeccionCiclos))]
    public virtual Sede Sede { get; set; } = null!;
}
