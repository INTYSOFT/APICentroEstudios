using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Models.Logistica.Producto;

[Table("lg_tipo_contacto")]
public partial class LgTipoContacto
{
    [Key]
    [Column("tipo_contacto_id")]
    public int TipoContactoId { get; set; }

    [Column("tipo_contacto")]
    [StringLength(50)]
    public string TipoContacto { get; set; } = null!;

    [Column("descripcion")]
    [StringLength(100)]
    public string? Descripcion { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [InverseProperty("TipoContacto")]
    public virtual ICollection<LgContactoCliente> LgContactoClientes { get; set; } = new List<LgContactoCliente>();
}
