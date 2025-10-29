using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_intiSoft.Models.CentroEstudios;

[Table("sede", Schema = "academia")]
public partial class Sede
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nombre")]
    public string Nombre { get; set; } = null!;

    [Column("ubigeo_code")]
    [StringLength(6)]
    public string UbigeoCode { get; set; } = null!;

    [Column("direccion")]
    public string? Direccion { get; set; }

    [Column("activo")]
    public bool Activo { get; set; }

    [Column("fecha_registro")]
    public DateTime? FechaRegistro { get; set; }

    [Column("fecha_actualizacion")]
    public DateTime? FechaActualizacion { get; set; }

    [Column("usuaraio_registro_id")]
    public int? UsuaraioRegistroId { get; set; }

    [Column("usuaraio_actualizacion_id")]
    public int? UsuaraioActualizacionId { get; set; }


    [InverseProperty("Sede")]
    public virtual ICollection<Orden> Ordens { get; set; } = new List<Orden>();

    //evaluacion
    [InverseProperty("Sede")]
    public virtual ICollection<Evaluacion> Evaluacions { get; set; } = new List<Evaluacion>();


}
