using api_intiSoft.Models.Common;

namespace api_intiSoft.Models.Logistica.Producto;

public partial class Marca : AuditableEntity
{
    public int MarcaId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? CodigoMarca { get; set; }

    public string? LogoUrl { get; set; }

    public string? SitioWeb { get; set; }

    public string? NumeroRegistro { get; set; }

    public int? TipoMarcaId { get; set; }

    public string? Descripcion { get; set; }

    public bool? Activo { get; set; }
}
