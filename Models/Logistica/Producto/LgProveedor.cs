using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_proveedor")]
public partial class LgProveedor
{
    [Key]
    [Column("proveedor_id")]
    public int ProveedorId { get; set; }

    [Column("documento_identidad_id")]
    [StringLength(3)]
    public string DocumentoIdentidadId { get; set; } = null!;

    [Column("nombre")]
    [StringLength(255)]
    public string Nombre { get; set; } = null!;

    [Column("direccion")]
    [StringLength(255)]
    public string? Direccion { get; set; }

    [Column("geo_latitud_llegada")]
    [Precision(10, 6)]
    public decimal GeoLatitudLlegada { get; set; }

    [Column("geo_longitud_llegada")]
    [Precision(10, 6)]
    public decimal GeoLongitudLlegada { get; set; }

    [Column("telefono")]
    [StringLength(50)]
    public string? Telefono { get; set; }

    [Column("email")]
    [StringLength(100)]
    public string? Email { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [ForeignKey("DocumentoIdentidadId")]
    [InverseProperty("LgProveedors")]
    public virtual LgDocumentoIdentidad DocumentoIdentidad { get; set; } = null!;

    [InverseProperty("Proveedor")]
    public virtual ICollection<LgOrdenCompra> LgOrdenCompras { get; set; } = new List<LgOrdenCompra>();
}
