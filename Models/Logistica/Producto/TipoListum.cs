namespace api_intiSoft.Models.Logistica.Producto;

public partial class TipoListum
{
    public short TipoListaId { get; set; }
    public string Nombre { get; set; } = null!;
    public string? Descripcion { get; set; }
    public bool? Activo { get; set; }

    // (Opcional) navegación inversa si quieres enlazar con ListaFichaTecnica:
    // public virtual ICollection<ListaFichaTecnica> ListaFichaTecnicas { get; set; } = new List<ListaFichaTecnica>();
}
