using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_almacen_sucursal")]
public partial class LgAlmacenSucursal
{
    [Key]
    [Column("almacen_sucursal_id")]
    public int AlmacenSucursalId { get; set; }

    [Column("almacen_id")]
    public int AlmacenId { get; set; }

    [Column("sucursal_id")]
    public int SucursalId { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [ForeignKey("AlmacenId")]
    [InverseProperty("LgAlmacenSucursals")]
    public virtual LgAlmacen Almacen { get; set; } = null!;

    [ForeignKey("SucursalId")]
    [InverseProperty("LgAlmacenSucursals")]
    public virtual LgSucursal Sucursal { get; set; } = null!;
}
