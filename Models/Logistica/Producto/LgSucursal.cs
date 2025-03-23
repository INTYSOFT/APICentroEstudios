using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_sucursal")]
public partial class LgSucursal
{
    [Key]
    [Column("sucursal_id")]
    public int SucursalId { get; set; }

    [Column("serie")]
    [StringLength(8)]
    public string Serie { get; set; } = null!;

    [Column("nombre")]
    [StringLength(50)]
    public string Nombre { get; set; } = null!;

    [Column("codigo_local_emisor")]
    [StringLength(4)]
    public string CodigoLocalEmisor { get; set; } = null!;

    [Column("geo_latitud_origen")]
    [Precision(10, 6)]
    public decimal GeoLatitudOrigen { get; set; }

    [Column("geo_longitud_origen")]
    [Precision(10, 6)]
    public decimal GeoLongitudOrigen { get; set; }

    [Column("direccion")]
    [StringLength(100)]
    public string? Direccion { get; set; }

    [Column("telefono")]
    [StringLength(50)]
    public string? Telefono { get; set; }

    [Column("email")]
    [StringLength(100)]
    public string? Email { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [InverseProperty("Sucursal")]
    public virtual ICollection<LgAlmacenSucursal> LgAlmacenSucursals { get; set; } = new List<LgAlmacenSucursal>();

    [InverseProperty("Sucursal")]
    public virtual ICollection<LgVentum> LgVenta { get; set; } = new List<LgVentum>();
}
