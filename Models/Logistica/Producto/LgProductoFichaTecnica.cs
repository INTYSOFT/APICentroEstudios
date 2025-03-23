using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_producto_ficha_tecnica")]
public partial class LgProductoFichaTecnica
{
    [Key]
    [Column("producto_ficha_tecnica_id")]
    public int ProductoFichaTecnicaId { get; set; }

    [ForeignKey("Producto")]
    [Column("producto_id")]
    public int ProductoId { get; set; }
    public virtual LgProducto Producto { get; set; }

    [ForeignKey("ProductoVariante")]
    [Column("producto_variante_id")]
    public int? ProductoVarianteId { get; set; }
    public virtual LgProductoVariante ProductoVariante { get; set; }

    [ForeignKey("FichaTecnica")]
    [Column("ficha_tecnica_id")]
    public int FichaTecnicaId { get; set; }
    public virtual LgFichaTecnica FichaTecnica { get; set; }

    [Required]
    [Column("valor")]
    public string Valor { get; set; }

    [Column("descripcion")]
    public string Descripcion { get; set; }

    [Required]
    [Column("activo")]
    public bool Activo { get; set; } = true;

    [ForeignKey("CategoriaFichaTecnicaDetalle")]
    [Column("categoria_ficha_tecnica_detalle_id")]
    public int CategoriaFichaTecnicaDetalleId { get; set; }
    public virtual LgCategoriaFichaTecnicaDetalle CategoriaFichaTecnicaDetalle { get; set; }

    [ForeignKey("CategoriaFichaTecnicaDetalleValor")]
    [Column("categoria_ficha_tecnica_detalle_valor_id")]
    public int CategoriaFichaTecnicaDetalleValorId { get; set; }
    public virtual LgCategoriaFichaTecnicaDetalleValor CategoriaFichaTecnicaDetalleValor { get; set; }

    [ForeignKey("CategoriaFichaTecnica")]
    [Column("categoria_ficha_tecnica_id")]
    public int? CategoriaFichaTecnicaId { get; set; }
    public virtual LgCategoriaFichaTecnica CategoriaFichaTecnica { get; set; }
}
