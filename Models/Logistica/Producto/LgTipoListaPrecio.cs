using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_tipo_lista_precio")]
public partial class LgTipoListaPrecio
{
    [Key]
    [Column("tipo_lista_precio_id")]
    public short TipoListaPrecioId { get; set; }

    [Column("tipo_lista_precio")]
    [StringLength(32)]
    public string? TipoListaPrecio { get; set; }

    [Column("descripcion")]
    [StringLength(128)]
    public string? Descripcion { get; set; }

    [Column("with_porcentaje")]
    public bool? WithPorcentaje { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [InverseProperty("TipoListaPrecio")]
    public virtual ICollection<LgListaPrecio> LgListaPrecios { get; set; } = new List<LgListaPrecio>();
}
