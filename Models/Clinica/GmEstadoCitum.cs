using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Clinica;

[Table("gm_estado_cita")]
public partial class GmEstadoCitum
{
    [Key]
    [Column("estado_cita_id")]
    public int EstadoCitaId { get; set; }

    [Column("estado")]
    [StringLength(50)]
    public string Estado { get; set; } = null!;

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [InverseProperty("EstadoCita")]
    public virtual ICollection<GmCitaMedica> GmCitaMedicas { get; set; } = new List<GmCitaMedica>();
}
