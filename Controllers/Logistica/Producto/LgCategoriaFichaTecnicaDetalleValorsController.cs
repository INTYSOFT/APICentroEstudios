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
    public class LgCategoriaFichaTecnicaDetalleValorsController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public LgCategoriaFichaTecnicaDetalleValorsController(ConecDinamicaContext context)
        {
            _context = context;
        }

        //By categoriaFichaTecnicaDetalleId
        [HttpGet("ByCategoriaFichaTecnicaDetalleId/{categoriaFichaTecnicaDetalleId}")]
        public async Task<ActionResult<IEnumerable<LgCategoriaFichaTecnicaDetalleValor>>> GetLgCategoriaFichaTecnicaDetalleValorByCategoriaFichaTecnicaDetalleId(int categoriaFichaTecnicaDetalleId)
        {
            return await _context.LgCategoriaFichaTecnicaDetalleValor.Where(x => x.CategoriaFichaTecnicaDetalleId == categoriaFichaTecnicaDetalleId).ToListAsync();
        }

        // GET: api/LgCategoriaFichaTecnicaDetalleValors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LgCategoriaFichaTecnicaDetalleValor>>> GetLgCategoriaFichaTecnicaDetalleValor()
        {
            return await _context.LgCategoriaFichaTecnicaDetalleValor.ToListAsync();
        }

        // GET: api/LgCategoriaFichaTecnicaDetalleValors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LgCategoriaFichaTecnicaDetalleValor>> GetLgCategoriaFichaTecnicaDetalleValor(int id)
        {
            var lgCategoriaFichaTecnicaDetalleValor = await _context.LgCategoriaFichaTecnicaDetalleValor.FindAsync(id);

            if (lgCategoriaFichaTecnicaDetalleValor == null)
            {
                return NotFound();
            }

            return lgCategoriaFichaTecnicaDetalleValor;
        }

        // PUT: api/LgCategoriaFichaTecnicaDetalleValors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLgCategoriaFichaTecnicaDetalleValor(int id, LgCategoriaFichaTecnicaDetalleValor lgCategoriaFichaTecnicaDetalleValor)
        {
            if (id != lgCategoriaFichaTecnicaDetalleValor.CategoriaFichaTecnicaDetalleValorId)
            {
                return BadRequest();
            }

            _context.Entry(lgCategoriaFichaTecnicaDetalleValor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LgCategoriaFichaTecnicaDetalleValorExists(id))
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

        // POST: api/LgCategoriaFichaTecnicaDetalleValors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LgCategoriaFichaTecnicaDetalleValor>> PostLgCategoriaFichaTecnicaDetalleValor(LgCategoriaFichaTecnicaDetalleValor lgCategoriaFichaTecnicaDetalleValor)
        {
            _context.LgCategoriaFichaTecnicaDetalleValor.Add(lgCategoriaFichaTecnicaDetalleValor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLgCategoriaFichaTecnicaDetalleValor", new { id = lgCategoriaFichaTecnicaDetalleValor.CategoriaFichaTecnicaDetalleValorId }, lgCategoriaFichaTecnicaDetalleValor);
        }

        // DELETE: api/LgCategoriaFichaTecnicaDetalleValors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLgCategoriaFichaTecnicaDetalleValor(int id)
        {
            var lgCategoriaFichaTecnicaDetalleValor = await _context.LgCategoriaFichaTecnicaDetalleValor.FindAsync(id);
            if (lgCategoriaFichaTecnicaDetalleValor == null)
            {
                return NotFound();
            }

            _context.LgCategoriaFichaTecnicaDetalleValor.Remove(lgCategoriaFichaTecnicaDetalleValor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LgCategoriaFichaTecnicaDetalleValorExists(int id)
        {
            return _context.LgCategoriaFichaTecnicaDetalleValor.Any(e => e.CategoriaFichaTecnicaDetalleValorId == id);
        }
    }
}
