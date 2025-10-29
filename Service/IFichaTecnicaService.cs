using api_intiSoft.Dto.Logistica.Producto;
using api_intiSoft.Models.Logistica.Producto;

namespace api_intiSoft.Service
{
    public interface IFichaTecnicaService
    {
        Task<List<CategoriaFichaTecnicaDetalleDto>> GetCategoriaFichaByCategoriaId(int categoriaId);
        //Task<List<CategoriaFichaTecnicaDetalleDto>> GetCategoriaFichaBWithCategoria();
    }


}
