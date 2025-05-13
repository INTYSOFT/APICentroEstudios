using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Clinica;

[Table("gm_especialidad")]
[Index("Nombre", Name = "gm_especialidad_nombre_key", IsUnique = true)]
public partial class GmEspecialidad
{
    [Key]
    [Column("especialidad_id")]
    public int EspecialidadId { get; set; }

    [Column("nombre")]
    [StringLength(100)]
    public string Nombre { get; set; } = null!;

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [InverseProperty("Especialidad")]
    public virtual ICollection<GmMedico> GmMedicos { get; set; } = new List<GmMedico>();
}
