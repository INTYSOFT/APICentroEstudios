using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_tipo_unidad_medida")]
public partial class LgTipoUnidadMedidum
{
    [Key]
    [Column("tipo_unidad_medida_id")]
    public int TipoUnidadMedidaId { get; set; }

    [Column("nombre")]
    [StringLength(50)]
    public string Nombre { get; set; } = null!;

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [InverseProperty("TipoUnidadMedida")]
    public virtual ICollection<LgUnidadMedidum> LgUnidadMedida { get; set; } = new List<LgUnidadMedidum>();
}
