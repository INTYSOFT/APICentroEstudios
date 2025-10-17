using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.CentroEstudios;

[Table("evaluacion_programada", Schema = "academia")]
[Index("CarreraId", Name = "ix_ep_carrera")]
[Index("CicloId", Name = "ix_ep_ciclo")]
[Index("NivelId", Name = "ix_ep_nivel")]
[Index("SeccionId", Name = "ix_ep_seccion")]
[Index("SeccionCicloId", Name = "ix_ep_seccion_c")]
[Index("SedeId", Name = "ix_ep_sede")]
[Index("TipoEvaluacionId", Name = "ix_ep_tipo")]
public partial class EvaluacionProgramadum
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("sede_id")]
    public int SedeId { get; set; }

    [Column("ciclo_id")]
    public int? CicloId { get; set; }

    [Column("tipo_evaluacion_id")]
    public int TipoEvaluacionId { get; set; }

    [Column("nombre")]
    public string Nombre { get; set; } = null!;

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

    [Column("seccion_id")]
    public int? SeccionId { get; set; }

    [Column("seccion_ciclo_id")]
    public int? SeccionCicloId { get; set; }

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

    [InverseProperty(nameof(EvaluacionDetalle.EvaluacionProgramada))]
    public virtual ICollection<EvaluacionDetalle> EvaluacionDetalles { get; set; } = new List<EvaluacionDetalle>();

    [InverseProperty(nameof(EvaluacionForma.EvaluacionProgramada))]
    public virtual ICollection<EvaluacionForma> EvaluacionFormas { get; set; } = new List<EvaluacionForma>();

    [InverseProperty(nameof(EvaluacionNotum.EvaluacionProgramada))]
    public virtual ICollection<EvaluacionNotum> EvaluacionNota { get; set; } = new List<EvaluacionNotum>();

    [InverseProperty(nameof(Evaluacion.EvaluacionProgramada))]
    public virtual ICollection<Evaluacion> Evaluacions { get; set; } = new List<Evaluacion>();

    [ForeignKey(nameof(TipoEvaluacionId))]
    [InverseProperty(nameof(TipoEvaluacion.EvaluacionProgramada))]
    public virtual TipoEvaluacion TipoEvaluacion { get; set; } = null!;

    [ForeignKey(nameof(CarreraId))]
    [InverseProperty(nameof(Carrera.EvaluacionProgramada))]
    public virtual Carrera? Carrera { get; set; }

    [ForeignKey(nameof(CicloId))]
    [InverseProperty(nameof(Ciclo.EvaluacionProgramada))]
    public virtual Ciclo? Ciclo { get; set; }

    [ForeignKey(nameof(NivelId))]
    [InverseProperty(nameof(Nivel.EvaluacionProgramada))]
    public virtual Nivel? Nivel { get; set; }

    [ForeignKey(nameof(SeccionId))]
    [InverseProperty(nameof(Seccion.Programaciones))]
    public virtual Seccion? Seccion { get; set; }

    [ForeignKey(nameof(SeccionCicloId))]
    [InverseProperty(nameof(SeccionCiclo.EvaluacionProgramada))]
    public virtual SeccionCiclo? SeccionCiclo { get; set; }

    [ForeignKey(nameof(SedeId))]
    [InverseProperty(nameof(Sede.EvaluacionProgramada))]
    public virtual Sede Sede { get; set; } = null!;
}
