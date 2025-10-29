using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_forma_pago")]
public partial class LgFormaPago
{
    [Key]
    [Column("forma_pago_id")]
    public int FormaPagoId { get; set; }

    [Column("forma_pago")]
    [StringLength(50)]
    public string FormaPago { get; set; } = null!;

    [Column("activo")]
    public bool? Activo { get; set; }

    [InverseProperty("FormaPago")]
    public virtual ICollection<LgVentum> LgVenta { get; set; } = new List<LgVentum>();
}
