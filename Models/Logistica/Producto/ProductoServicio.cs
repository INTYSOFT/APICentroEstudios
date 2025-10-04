using api_intiSoft.Models.Common;

namespace api_intiSoft.Models.Logistica.Producto;

public partial class ProductoServicio : AuditableEntity
{
    public int ProductoServicioId { get; set; }

    public int ProductoId { get; set; }

    public int? Dias { get; set; }
    public int? Horas { get; set; }
    public int? Minutos { get; set; }
    public int? Segundos { get; set; }

    // Navegación requerida
    public virtual Producto Producto { get; set; } = null!;
}
