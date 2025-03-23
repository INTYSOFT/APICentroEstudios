using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_producto_variante")]
[Index("CodigoBarras", Name = "idx_codigo_barras", IsUnique = true)]
[Index("CodigoQr", Name = "idx_codigo_qr")]
[Index("VarianteSku", Name = "lg_variante_producto_variante_sku_key", IsUnique = true)]
public partial class LgProductoVariante
{
    [Key]
    [Column("producto_variante_id")]
    public int ProductoVarianteId { get; set; }

    [Column("producto_id")]
    public int ProductoId { get; set; }

    [Column("variante_sku")]
    [StringLength(32)]
    public string VarianteSku { get; set; } = null!;

    [Column("nombre")]
    [StringLength(255)]
    public string? Nombre { get; set; }

    [Column("atributos", TypeName = "jsonb")]
    public string? Atributos { get; set; }

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [Column("codigo_barras")]
    [StringLength(32)]
    public string? CodigoBarras { get; set; }

    [Column("codigo_qr")]
    public byte[]? CodigoQr { get; set; }

    [Column("codigo_interno")]
    [StringLength(32)]
    public string? CodigoInterno { get; set; }

    [Column("empaque_id")]
    public int? EmpaqueId { get; set; }

    [Column("largo")]
    [Precision(10, 2)]
    public decimal? Largo { get; set; }

    [Column("ancho")]
    [Precision(10, 2)]
    public decimal? Ancho { get; set; }

    [Column("altura")]
    [Precision(10, 2)]
    public decimal? Altura { get; set; }

    [Column("unidad_medida_longitud_id")]
    public int? UnidadMedidaLongitudId { get; set; }

    [Column("peso")]
    [Precision(10, 2)]
    public decimal? Peso { get; set; }

    [Column("unidad_medida_peso_id")]
    public int? UnidadMedidaPesoId { get; set; }

    [Column("categoria_ficha_tecnica_detalle_id")]
    public int? CategoriaFichaTecnicaDetalleId { get; set; }

    [Column("incluye_variante")]
    public bool IncluyeVariante { get; set; } = false;

    [Column("producto_variante_id_incluye")]
    public int? ProductoVarianteIdIncluye { get; set; }

    [Column("unidad_medida_contenido_id")]
    public int? UnidadMedidaContenidoId { get; set; }

    [Column("contenido")]
    [Precision(10, 2)]
    public decimal? Contenido { get; set; }

    [Column("cantidad_incluye_variante")]
    [Precision(10, 2)]
    public decimal? CantidadIncluyeVariante { get; set; }


    [InverseProperty("lgProductoVariantes")]
    public LgProducto? Producto { get; set; }


    public int Id { get; set; }
    public ICollection<LgDetalleGuiaRemision>? LgDetalleGuiaRemisions { get; set; }

    public int Ids { get; set; }

    [InverseProperty("VarianteProducto")]
    public ICollection<LgInventario>? LgInventarios { get; set; }


    [InverseProperty("VarianteProducto")]
    public virtual ICollection<LgItemOrdenCompra> LgItemOrdenCompras { get; set; } = new List<LgItemOrdenCompra>();

    public int Idss { get; set; }

    [InverseProperty("ProductoVariante")]
    public virtual ICollection<LgItemVentum>? LgItemVenta { get; set; }

    public int MovimientoStockId { get; set; }

    public ICollection<LgMovimientoStock>? LgMovimientoStocks { get; set; }


    public int PrecioId { get; set; }
    public ICollection<LgPrecio>? LgPrecios { get; set; }

    public ICollection<LgPromocionProducto>? LgPromocionProductos { get; set; }


}
