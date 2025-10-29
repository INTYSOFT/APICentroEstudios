using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_almacen")]
public partial class LgAlmacen
{
    [Key]
    [Column("almacen_id")]
    public int AlmacenId { get; set; }

    [Column("nombre")]
    [StringLength(255)]
    public string Nombre { get; set; } = null!;

    [Column("ubicacion")]
    [StringLength(255)]
    public string? Ubicacion { get; set; }

    [Column("geo_latitud_origen")]
    [Precision(10, 6)]
    public decimal GeoLatitudOrigen { get; set; }

    [Column("geo_longitud_origen")]
    [Precision(10, 6)]
    public decimal GeoLongitudOrigen { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [InverseProperty("Almacen")]
    public virtual ICollection<LgAlmacenSucursal> LgAlmacenSucursals { get; set; } = new List<LgAlmacenSucursal>();

    [InverseProperty("Almacen")]
    public virtual ICollection<LgInventario> LgInventarios { get; set; } = new List<LgInventario>();

    [InverseProperty("Almacen")]
    public virtual ICollection<LgMovimientoStock> LgMovimientoStocks { get; set; } = new List<LgMovimientoStock>();
}
