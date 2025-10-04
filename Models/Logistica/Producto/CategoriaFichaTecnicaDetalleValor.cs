namespace api_intiSoft.Models.Logistica.Producto;

public partial class CategoriaFichaTecnicaDetalleValor
{
    public int CategoriaFichaTecnicaDetalleValorId { get; set; }

    public int CategoriaFichaTecnicaDetalleId { get; set; }

    public string Valor { get; set; } = null!;

    public string? Descripcion { get; set; }

    public int Version { get; set; } = 1;

    public bool Activo { get; set; } = true;

    // ==== Navegaciones ====
    public virtual CategoriaFichaTecnicaDetalle CategoriaFichaTecnicaDetalle { get; set; } = null!;
}
