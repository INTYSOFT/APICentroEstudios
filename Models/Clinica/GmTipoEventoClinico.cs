using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Clinica;

[Table("gm_tipo_evento_clinico")]
[Index("Nombre", Name = "gm_tipo_evento_clinico_nombre_key", IsUnique = true)]
public partial class GmTipoEventoClinico
{
    [Key]
    [Column("tipo_evento_id")]
    public int TipoEventoId { get; set; }

    [Column("nombre")]
    [StringLength(100)]
    public string Nombre { get; set; } = null!;

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [InverseProperty("TipoEvento")]
    public virtual ICollection<GmHistoriaClinicaEvento> GmHistoriaClinicaEventos { get; set; } = new List<GmHistoriaClinicaEvento>();
}
