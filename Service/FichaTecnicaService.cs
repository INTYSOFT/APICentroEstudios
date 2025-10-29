using api_intiSoft.Dto.Logistica.Producto;
using api_intiSoft.Models.Logistica.Producto;
using intiSoft;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Service
{
    public class FichaTecnicaService : IFichaTecnicaService
    {
        private readonly ConecDinamicaContext _context;

        public FichaTecnicaService(ConecDinamicaContext context)
        {
            _context = context;
        }


        //public async Task<List<FichaTecnicaWithFichaTecnicaDto>> GetCategoriaFichaBWithCategoria()
        //{
        //var fichasTecnicas = await _context.LgCategoriaFichaTecnica
        //    .Select(x => new FichaTecnicaWithFichaTecnicaDto
        //    {
        //        CategoriaFichaTecnicaId = x.CategoriaFichaTecnicaId,
        //        FichaTecnicaId = x.FichaTecnicaId,
        //        CategoriaId = x.CategoriaId,
        //        IsRequirido = x.IsRequirido,
        //        WithLista = x.WithLista,
        //        Version = x.Version,
        //        Activo = x.Activo,
        //        FichaTecnica = new FichaTecnicaDto
        //        {
        //            FichaTecnicaId = x.FichaTecnica.FichaTecnicaId,
        //            Nombre = x.FichaTecnica.Nombre,
        //            Descripcion = x.FichaTecnica.Descripcion,
        //            Activo = x.FichaTecnica.Activo
        //        }
        //    })
        //    .ToListAsync();

        //    return null;
        //}

        public async Task<List<CategoriaFichaTecnicaDetalleDto>> GetCategoriaFichaByCategoriaId(int categoriaId)
        {
            var categoriaFichaTecnica = await _context.LgCategoriaFichaTecnica
                .Where(p => p.CategoriaId == categoriaId)
                .Select(p => new CategoriaFichaTecnicaDetalleDto
                {
                    CategoriaFichaTecnicaId = p.CategoriaFichaTecnicaId,
                    CategoriaId = p.CategoriaId,
                    FichaTecnicaId = p.FichaTecnicaId,
                    Activo = p.Activo,
                    FichaTecnica = p.FichaTecnica,
                    Categoria = p.Categoria,                    
                    CategoriaFichaTecnicaDetalle = p.CategoriaFichaTecnicaDetalles.ToList()
                    
                })
                .ToListAsync();

            return categoriaFichaTecnica;
        }



    }
}
