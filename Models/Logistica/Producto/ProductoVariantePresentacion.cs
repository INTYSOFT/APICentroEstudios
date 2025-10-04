using System;
using System.Collections.Generic;
using NpgsqlTypes;

namespace api_intiSoft.Models.Logistica.Producto;

public partial class ProductoVariantePresentacion
{
    public int ProductoVariantePresentacionId { get; set; }
    public int ProductoId { get; set; }
    public int? ProductoVarianteDetalleId { get; set; }
    public int? ProductoPresentacionId { get; set; }

    public string Sku { get; set; } = null!;
    public string? CodigoBarra { get; set; }
    public string? CodigoQr { get; set; }
    public string? CodigoInterno { get; set; }

    public decimal PrecioCompra { get; set; }
    public decimal PrecioVenta { get; set; }
    public decimal? CostoFlete { get; set; }
    public decimal? CostoAdicional { get; set; }
    public decimal? DescuentoPorcentaje { get; set; }
    public decimal? PrecioVentaFinal { get; set; }

    public int Stock { get; set; }
    public int? StockMinimo { get; set; }
    public int? StockIdeal { get; set; }
    public int? StockMaximo { get; set; }

    public bool Activo { get; set; }

    public DateTime? FechaRegistro { get; set; }
    public DateTime? FechaActualizacion { get; set; }
    public int? UsuaraioRegistroId { get; set; }
    public int? UsuaraioActualizacionId { get; set; }

    public string? Aliass { get; set; }
    public string? NombreProductoCorto { get; set; }
    public string? NombreProductoCompleto { get; set; }
    public string? NombreProductoComplejo { get; set; }
    public string? EtiquetaVenta { get; set; }

    public NpgsqlTsVector? NombreBusqueda { get; set; }
    public string? Nombre { get; set; }
    public string? Descripcion { get; set; }

    // ==== Navegaciones ====
    public virtual Producto? Producto { get; set; } = null!;
    public virtual ProductoVarianteDetalle? ProductoVarianteDetalle { get; set; }
    public virtual ProductoPresentacion? ProductoPresentacion { get; set; }

    public virtual ICollection<ProductoVariantePresentacionDet?> Detalles { get; set; }        = new List<ProductoVariantePresentacionDet>();
}
