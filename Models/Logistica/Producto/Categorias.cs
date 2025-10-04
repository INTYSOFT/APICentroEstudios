using api_intiSoft.Models.Common;

namespace api_intiSoft.Models.Logistica.Producto;

public partial class Categorias : AuditableEntity
{
    public int CategoriaId { get; set; }

    public int? CategoriaMainId { get; set; }

    public bool? IsEnd { get; set; }

    public string Categoria { get; set; } = null!;

    public string? Descripcion { get; set; }

    public bool? Activo { get; set; }

    public bool? IsEditadoFichaTecnica { get; set; }

    public int? CategoriaIdFichaTecnica { get; set; }
    //orden Integer
    public int? Orden { get; set; }

    //aliass varchar(5)
    public string? Aliass { get; set; }

    // Relación inversa con Producto (1:N)
    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();

    // Colección adicional
    public virtual ICollection<CategoriaFichaTecnica> CategoriaFichaTecnicas { get; set; } = new List<CategoriaFichaTecnica>();
}
