using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_estado_guia")]
[Index("NombreEstado", Name = "lg_estado_guia_nombre_estado_key", IsUnique = true)]
public partial class LgEstadoGuium
{
    [Key]
    [Column("estado_id")]
    public int EstadoId { get; set; }

    [Column("nombre_estado")]
    [StringLength(50)]
    public string NombreEstado { get; set; } = null!;

    [Column("descripcion")]
    [StringLength(255)]
    public string? Descripcion { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [InverseProperty("Estado")]
    public virtual ICollection<LgHistorialEstadoGuium> LgHistorialEstadoGuia { get; set; } = new List<LgHistorialEstadoGuium>();
}
