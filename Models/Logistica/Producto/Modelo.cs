using api_intiSoft.Models.Common;

namespace api_intiSoft.Models.Logistica.Producto;

public partial class Modelo : AuditableEntity
{
    public int ModeloId { get; set; }
    public string Nombre { get; set; } = null!;
    public string? CodigoModelo { get; set; }
    public string? Descripcion { get; set; }
    public bool Activo { get; set; }
}
