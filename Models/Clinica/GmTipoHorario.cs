using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Clinica;

[Table("gm_tipo_horario")]
[Index("Codigo", Name = "gm_tipo_horario_codigo_key", IsUnique = true)]
public partial class GmTipoHorario
{
    [Key]
    [Column("tipo_horario_id")]
    public short TipoHorarioId { get; set; }

    [Column("codigo")]
    [StringLength(20)]
    public string Codigo { get; set; } = null!;

    [Column("nombre")]
    [StringLength(100)]
    public string Nombre { get; set; } = null!;

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [InverseProperty("TipoHorario")]
    public virtual ICollection<GmHorarioApertura> GmHorarioAperturas { get; set; } = new List<GmHorarioApertura>();
}
