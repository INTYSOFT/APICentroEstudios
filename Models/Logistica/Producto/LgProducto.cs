using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_producto")]
public partial class LgProducto
{
    [Key]
    [Column("producto_id")]
    public int ProductoId { get; set; }

    [Column("categoria_id")]
    public int? CategoriaId { get; set; }

    [Column("plan_contable_id")]
    public int? PlanContableId { get; set; }

    [Column("codigo_sunat")]
    [StringLength(16)]
    public string? CodigoSunat { get; set; }

    [Column("nombre")]
    [StringLength(255)]
    public string Nombre { get; set; } = null!;

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    //marca_id
    [Column("marca_id")]
    public int? MarcaId { get; set; }

    [Column("is_producto")]
    public bool? IsProducto { get; set; }

    [Column("tipo_transaccion_id")]
    public short? TipoTransaccionId { get; set; }

    [Column("lista_precio_id")]
    public short? ListaPrecioId { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [ForeignKey("CategoriaId")]
    [InverseProperty("LgProductos")]
    public virtual LgCategoria? Categoria { get; set; }



    
    [InverseProperty("Producto")]
    public virtual ICollection<LgDetalleGuiaRemision> LgDetalleGuiaRemisions { get; set; } = new List<LgDetalleGuiaRemision>();

    [InverseProperty("Producto")]
    public virtual ICollection<LgEmpaqueProducto> LgEmpaqueProductos { get; set; } = new List<LgEmpaqueProducto>();

    [InverseProperty("Producto")]
    public virtual ICollection<LgInventario> LgInventarios { get; set; } = new List<LgInventario>();

    [InverseProperty("Producto")]
    public virtual ICollection<LgItemOrdenCompra> LgItemOrdenCompras { get; set; } = new List<LgItemOrdenCompra>();

    [InverseProperty("Producto")]
    public virtual ICollection<LgItemVentum> LgItemVenta { get; set; } = new List<LgItemVentum>();

    [InverseProperty("Producto")]
    public virtual ICollection<LgMovimientoStock> LgMovimientoStocks { get; set; } = new List<LgMovimientoStock>();

    [InverseProperty("Producto")]
    public virtual ICollection<LgPrecio> LgPrecios { get; set; } = new List<LgPrecio>();

    
    [InverseProperty("Producto")]
    public virtual ICollection<LgProductoFichaTecnica> LgProductoFichaTecnicas { get; set; } = new List<LgProductoFichaTecnica>();

    [InverseProperty("Producto")]
    public ICollection<LgProductoVariante>? lgProductoVariantes { get; set; }
        


    [InverseProperty("Producto")]
    public virtual ICollection<LgPromocionProducto> LgPromocionProductos { get; set; } = new List<LgPromocionProducto>();

    [ForeignKey("TipoTransaccionId")]
    [InverseProperty("LgProductos")]
    public virtual LgTipoTransaccion? TipoTransaccion { get; set; }
    
}

//ProductoDto
public class ProductoDto
{
    public int ProductoId { get; set; }
    public string Producto { get; set; } = null!;
    public bool? Activo { get; set; }
}