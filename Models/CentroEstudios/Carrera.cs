using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_intiSoft.Models.CentroEstudios;

[Table("carrera", Schema = "academia")]
[Index("Nombre", Name = "ux_carrera_univ_nombre", IsUnique = true)]
public partial class Carrera
{
    [Key]
    [Column("id")]
    public int Id { get; set; }



    [Column("nombre")]
    public string Nombre { get; set; } = null!;

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


    //matriculas
    [InverseProperty("Carrera")]
    public virtual ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();
}
