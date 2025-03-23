using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_intiSoft.Models.Logistica.Producto;
using intiSoft;

namespace api_intiSoft.Controllers.Logistica.Producto
{
    [Route("api/[controller]")]
    [ApiController]
    public class LgProductoesController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public LgProductoesController(ConecDinamicaContext context)
        {
            _context = context;
        }

        //Get Solo devuleve el nombre primer producto encontrado por categoria_id
        [HttpGet("GetLgProductoByCategoriaId/{categoria_id}")]
        public async Task<ActionResult<string>> GetLgProductoByCategoriaId(int categoria_id)
        {
            //devuelve solo el nombre del producto
            var producto = await _context.LgProducto
                .Where(p => p.CategoriaId == categoria_id)
                .Select(p => new 
                {
                    Nombre = p.Nombre
                })
                .FirstOrDefaultAsync();

            return Ok(producto);

        }

        // GET: api/LgProductoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LgProducto>>> GetLgProducto()
        {
            return await _context.LgProducto.ToListAsync();
        }

        // GET: api/LgProductoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoDto>> GetLgProducto(int id)
        {
            //var lgProducto = await _context.LgProducto
            //    .Include(x => x.Categoria)
            //    .Include(x => x.PlanContable)
            //    .Include(x => x.TipoTransaccion)
            //    .FirstOrDefaultAsync(x => x.ProductoId == id);

            var lgProducto = await _context.LgProducto
            .Where(p => p.ProductoId == id)
            .Include(p => p.CategoriaId)
            .Select(p => new ProductoDto
            {
                //ProductoDto
                ProductoId = p.ProductoId,                
                Producto= p.Nombre,
                Activo = p.Activo

            })
            .FirstOrDefaultAsync();


            if (lgProducto == null)
            {
                return NotFound();
            }

            return lgProducto;

        }


        //[HttpGet("{id}")]
        //public async Task<ActionResult<ProductoDto>> GetLgProducto0002(int id)
        //{
        //    var producto = await _context.LgProducto
        //        .Include(p => p.Categoria)
        //        .Include(p => p.PlanContable)
        //        .Include(p => p.TipoTransaccion)
        //        .FirstOrDefaultAsync(p => p.ProductoId == id);

        //    if (producto == null)
        //    {
        //        return NotFound();
        //    }

        //    var productoDto = _mapper.Map<ProductoDto>(producto);

        //    return productoDto;
        //}

        // PUT: api/LgProductoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLgProducto(int id, LgProducto lgProducto)
        {
            if (id != lgProducto.ProductoId)
            {
                return BadRequest();
            }

            _context.Entry(lgProducto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LgProductoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/LgProductoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LgProducto>> PostLgProducto(LgProducto lgProducto)
        {
            _context.LgProducto.Add(lgProducto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLgProducto", new { id = lgProducto.ProductoId }, lgProducto);
        }

        // DELETE: api/LgProductoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLgProducto(int id)
        {
            var lgProducto = await _context.LgProducto.FindAsync(id);
            if (lgProducto == null)
            {
                return NotFound();
            }

            _context.LgProducto.Remove(lgProducto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LgProductoExists(int id)
        {
            return _context.LgProducto.Any(e => e.ProductoId == id);
        }
    }
}
