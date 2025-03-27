using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_producto_presentacion")]
[Index("CodigoBarras", Name = "idx_codigo_barras", IsUnique = true)]
[Index("CodigoQr", Name = "idx_codigo_qr")]

public partial class LgProductoPresentacion
{
    [Key]
    [Column("producto_presentacion_id")]
    public int ProductoPresentacionId { get; set; }

    [Column("producto_id")]
    public int ProductoId { get; set; }

    [Column("presentacion_sku")]
    [StringLength(32)]
    public string PresentacionSku { get; set; } = null!;

    [Column("nombre")]
    [StringLength(255)]
    public string? Nombre { get; set; }

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

    [Column("incluye_presentacion")]
    public bool IncluyePresentacion { get; set; }

    [Column("producto_presentacion_id_incluye")]
    public int? ProductoPresentacionIdIncluye { get; set; }

    [Column("unidad_medida_contenido_id")]
    public int? UnidadMedidaContenidoId { get; set; }

    [Column("contenido")]
    [Precision(10, 2)]
    public decimal? Contenido { get; set; }

    [Column("cantidad_incluye_presentacion")]
    [Precision(10, 2)]
    public decimal? CantidadIncluyePresentacion { get; set; }

    [Column("orden")]
    public short? Orden { get; set; }
}
