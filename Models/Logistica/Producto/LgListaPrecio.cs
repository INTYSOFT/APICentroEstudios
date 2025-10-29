using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_lista_precio")]
public partial class LgListaPrecio
{
    [Key]
    [Column("lista_precio_id")]
    public short ListaPrecioId { get; set; }

    [Column("lista_precio")]
    [StringLength(64)]
    public string? ListaPrecio { get; set; }

    [Column("tipo_lista_precio_id")]
    public short? TipoListaPrecioId { get; set; }

    [Column("precio")]
    [Precision(8, 2)]
    public decimal? Precio { get; set; }

    [Column("descripcion")]
    [StringLength(256)]
    public string? Descripcion { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [ForeignKey("TipoListaPrecioId")]
    [InverseProperty("LgListaPrecios")]
    public virtual LgTipoListaPrecio? TipoListaPrecio { get; set; }
}
