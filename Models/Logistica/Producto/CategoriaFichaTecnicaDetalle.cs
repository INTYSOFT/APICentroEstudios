using api_intiSoft.Models.Common;
using api_intiSoft.Models.Universal;

namespace api_intiSoft.Models.Logistica.Producto;

public partial class CategoriaFichaTecnicaDetalle : AuditableEntity
{
    public int CategoriaFichaTecnicaDetalleId { get; set; }

    public int? ListaFichaTecnicaId { get; set; }

    public int CategoriaFichaTecnicaId { get; set; }

    // Flags y valores por defecto (no-nullables)
    public bool IsRequerido { get; set; } = false;
    public bool WithLista { get; set; } = false;
    public int Version { get; set; } = 1;
    public bool Activo { get; set; } = true;
    public bool PermitirIngreso { get; set; } = true;

    public string Nombre { get; set; } = null!;
    public string? Descripcion { get; set; }

    public int? TipoDatoId { get; set; }

    // ==== Navegaciones ====
    public virtual CategoriaFichaTecnica? CategoriaFichaTecnica { get; set; } = null!;

    // Si existen estas entidades en tu modelo, mantenemos las navegaciones opcionales:
    public virtual ListaFichaTecnica? ListaFichaTecnica { get; set; }
    public virtual UnTipoDato? TipoDato { get; set; }
}
