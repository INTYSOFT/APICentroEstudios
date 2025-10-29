using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_vehiculo")]
[Index("NumeroPlaca", Name = "lg_vehiculo_numero_placa_key", IsUnique = true)]
public partial class LgVehiculo
{
    [Key]
    [Column("vehiculo_id")]
    public int VehiculoId { get; set; }

    [Column("numero_placa")]
    [StringLength(20)]
    public string NumeroPlaca { get; set; } = null!;

    [Column("marca")]
    [StringLength(50)]
    public string? Marca { get; set; }

    [Column("modelo")]
    [StringLength(50)]
    public string? Modelo { get; set; }

    [Column("anio")]
    public int? Anio { get; set; }

    [Column("color")]
    [StringLength(20)]
    public string? Color { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [InverseProperty("Vehiculo")]
    public virtual ICollection<LgGuiaRemision> LgGuiaRemisions { get; set; } = new List<LgGuiaRemision>();
}
