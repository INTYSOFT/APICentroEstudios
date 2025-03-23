using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using intiSoft;
using api_intiSoft.Models.Logistica.Producto;
using Microsoft.AspNetCore.JsonPatch;
using api_intiSoft.Models.Universal;

namespace api_intiSoft.Controllers.Logistica.Producto
{
    [Route("api/[controller]")]
    [ApiController]
    public class LgCategoriumsController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public LgCategoriumsController(ConecDinamicaContext context)
        {
            _context = context;
        }

        //get por <categoria>
        [HttpGet("GetLgCategoriaByCategoria/{categoria}")]
        public async Task<ActionResult<String>> GetLgCategoriaByCategoria(string categoria)
        {
            //retornon el nombre de categoris si existe
            var lgCategoria = await _context.LgCategorium
                .Where(p => p.Categoria == categoria)
                .Select(p => new
                {
                    Categoria = p.Categoria
                })
                .FirstOrDefaultAsync();

            return Ok(lgCategoria);
        }



        // GET: api/LgCategoriums
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LgCategoria>>> GetLgCategorium()
        {
            return await _context.LgCategorium.ToListAsync();
        }

        // GET: api/LgCategoriums/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LgCategoria>> GetLgCategorium(int id)
        {
            var lgCategorium = await _context.LgCategorium.FindAsync(id);

            if (lgCategorium == null)
            {
                return NotFound();
            }

            return lgCategorium;
        }

        //Get Lgproductos by idCategoria
        [HttpGet("GetProductoByCategoria/{idCategoria}")]
        public async Task<ActionResult<IEnumerable<DatoGenericoDto>>> GetLgProductosByCategoria(int idCategoria)
        {
            //Solo un producto el primero
            var lgProducto = await _context.LgProducto
                .Where(p => p.CategoriaId == idCategoria)
                .Select(p => new DatoGenericoDto
                {
                    Id = p.ProductoId,
                    Nombre = p.Nombre
                })
                .FirstOrDefaultAsync();

            if (lgProducto == null)
            {
                return Ok(null);
            }

            return Ok(lgProducto);



        }

        // PUT: api/LgCategoriums/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLgCategorium(int id, LgCategoria lgCategorium)
        {
            if (id != lgCategorium.CategoriaId)
            {
                return BadRequest();
            }

            _context.Entry(lgCategorium).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LgCategoriumExists(id))
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

        //pacth int id, [FromBody] JsonPatchDocument<LgCategoriaTree> patchDoc)
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchLgCategorium(int id, [FromBody] JsonPatchDocument<LgCategoria> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var lgCategorium = await _context.LgCategorium.FirstOrDefaultAsync(x => x.CategoriaId == id);

            if (lgCategorium == null)
            {
                return NotFound();
            }

            patchDoc.ApplyTo(lgCategorium, ModelState);

            var isValid = TryValidateModel(lgCategorium);

            if (!isValid)
            {
                return BadRequest(ModelState);
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }


        // POST: api/LgCategoriums
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LgCategoria>> PostLgCategorium(LgCategoria lgCategorium)
        {
            _context.LgCategorium.Add(lgCategorium);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLgCategorium", new { id = lgCategorium.CategoriaId }, lgCategorium);
        }

        // DELETE: api/LgCategoriums/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLgCategorium(int id)
        {
            var lgCategorium = await _context.LgCategorium.FindAsync(id);
            if (lgCategorium == null)
            {
                return NotFound();
            }

            _context.LgCategorium.Remove(lgCategorium);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LgCategoriumExists(int id)
        {
            return _context.LgCategorium.Any(e => e.CategoriaId == id);
        }
    }
}
