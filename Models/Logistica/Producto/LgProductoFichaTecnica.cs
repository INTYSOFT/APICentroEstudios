using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

/// <summary>
/// Representa los datos técnicos asociados a un producto.

[Table("lg_producto_ficha_tecnica")]
public partial class LgProductoFichaTecnica
{
    [Key]
    [Column("producto_ficha_tecnica_id")]
    public int ProductoFichaTecnicaId { get; set; }

    [Column("producto_id")]
    public int ProductoId { get; set; }

    [Column("ficha_tecnica_id")]
    public int? FichaTecnicaId { get; set; }

    [Column("valor")]
    public string Valor { get; set; } = null!;

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("activo")]
    public bool Activo { get; set; }


    [Column("categoria_ficha_tecnica_detalle_id")]
    public int? CategoriaFichaTecnicaDetalleId { get; set; }

    [Column("categoria_ficha_tecnica_detalle_valor_id")]
    public int? CategoriaFichaTecnicaDetalleValorId { get; set; }

    [Column("categoria_ficha_tecnica_id")]
    public int? CategoriaFichaTecnicaId { get; set; }

    [Column("nombre")]
    [StringLength(128)]
    public string Nombre { get; set; }

    [Column("lista_ficha_tecnica_id")]
    public int? ListaFichaTecnicaId { get; set; }

    [Column("detalle_lista_ficha_tecnica_id")]
    public int? DetalleListaFichaTecnicaId { get; set; }

    [ForeignKey("ProductoId")]
    [InverseProperty("LgProductoFichaTecnicas")]
    public virtual LgProducto Producto { get; set; } = null!;


    [ForeignKey("FichaTecnicaId")]
    [InverseProperty("LgProductoFichaTecnicas")]
    public virtual LgFichaTecnica FichaTecnica { get; set; } = null!;

    [ForeignKey("CategoriaFichaTecnicaDetalleId")]
    public virtual LgCategoriaFichaTecnicaDetalle CategoriaFichaTecnicaDetalle { get; set; } = null!;

    [ForeignKey("CategoriaFichaTecnicaDetalleValorId")]
    public virtual LgCategoriaFichaTecnicaDetalleValor CategoriaFichaTecnicaDetalleValor { get; set; } = null!;

    [ForeignKey("CategoriaFichaTecnicaId")]
    public virtual LgCategoriaFichaTecnica? CategoriaFichaTecnica { get; set; }
}
