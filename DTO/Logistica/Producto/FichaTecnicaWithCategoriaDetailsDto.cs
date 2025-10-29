using api_intiSoft.Models.Logistica.Producto;

namespace api_intiSoft.Dto.Logistica.Producto
{
    public class CategoriaFichaTecnicaDetalleDto
    {
        //lgCategoriaFichaTecnica        
        public int CategoriaFichaTecnicaId { get; set; }
        public int? CategoriaId { get; set; }
        public int? FichaTecnicaId { get; set; }
        public bool? Activo { get; set; }        
        
        public LgFichaTecnica? FichaTecnica { get; set; }
        public LgCategoria? Categoria { get; set; }
        
        //lista public LgCategoriaFichaTecnicaDetalle? CategoriaFichaTecnicaDetalle { get; set; }

        public List<LgCategoriaFichaTecnicaDetalle?> CategoriaFichaTecnicaDetalle { get; set; }

    }

    public class FichaTecnicaDto
    {
        //LgFichaTecnica
        public int FichaTecnicaId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }

    }
}
