using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.CentroEstudios;

[Table("alumno_apoderado", Schema = "academia")]
[Index("AlumnoId", "ApoderadoId", Name = "ux_aa_unique", IsUnique = true)]
public partial class AlumnoApoderado
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("alumno_id")]
    public int AlumnoId { get; set; }

    [Column("apoderado_id")]
    public int ApoderadoId { get; set; }
    //parentesco_id
    [Column("parentesco_id")]
    public int? ParentescoId { get; set; }

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

    [ForeignKey(nameof(AlumnoId))]
    [InverseProperty(nameof(Alumno.AlumnoApoderados))]
    public virtual Alumno Alumno { get; set; } = null!;

    [ForeignKey(nameof(ApoderadoId))]
    [InverseProperty(nameof(Apoderado.AlumnoApoderados))]
    public virtual Apoderado Apoderado { get; set; } = null!;

    [ForeignKey(nameof(ParentescoId))]
    [InverseProperty(nameof(Parentesco.AlumnoApoderados))]
    public virtual Parentesco? Parentesco { get; set; }
}
