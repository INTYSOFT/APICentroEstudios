using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_empresa_asociada_producto")]
public partial class LgEmpresaAsociadaProducto
{
    [Key]
    [Column("empresa_asociada_producto_id")]
    public int EmpresaAsociadaProductoId { get; set; }

    [Column("empresa_asociada_id")]
    public int EmpresaAsociadaId { get; set; }

    [Column("producto_id")]
    public int ProductoId { get; set; }

    [Column("precio")]
    [Precision(10, 2)]
    public decimal Precio { get; set; }

    [Column("stock_disponible")]
    public int? StockDisponible { get; set; }

    [Column("fecha_registro", TypeName = "timestamp without time zone")]
    public DateTime? FechaRegistro { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }
}
