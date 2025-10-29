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
    public class LgProductoFichaTecnicasController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public LgProductoFichaTecnicasController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // producto by categoriaFichaTecnicaId
        [HttpGet("ByCategoriaFichaTecnicaId/{categoriaFichaTecnicaId}")]
        public async Task<ActionResult<IEnumerable<LgProductoFichaTecnica>>> GetLgProductoFichaTecnicaByCategoriaFichaTecnicaId(int categoriaFichaTecnicaId)
        {
            return await _context.LgProductoFichaTecnica.Where(p => p.CategoriaFichaTecnicaId == categoriaFichaTecnicaId).ToListAsync();
        }

        // GET: api/LgProductoFichaTecnicas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LgProductoFichaTecnica>>> GetLgProductoFichaTecnica()
        {
            return await _context.LgProductoFichaTecnica.ToListAsync();
        }

        // GET: api/LgProductoFichaTecnicas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LgProductoFichaTecnica>> GetLgProductoFichaTecnica(int id)
        {
            var lgProductoFichaTecnica = await _context.LgProductoFichaTecnica.FindAsync(id);

            if (lgProductoFichaTecnica == null)
            {
                return NotFound();
            }

            return lgProductoFichaTecnica;
        }

        // PUT: api/LgProductoFichaTecnicas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLgProductoFichaTecnica(int id, LgProductoFichaTecnica lgProductoFichaTecnica)
        {
            if (id != lgProductoFichaTecnica.ProductoFichaTecnicaId)
            {
                return BadRequest();
            }

            _context.Entry(lgProductoFichaTecnica).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LgProductoFichaTecnicaExists(id))
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

        // POST: api/LgProductoFichaTecnicas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LgProductoFichaTecnica>> PostLgProductoFichaTecnica(LgProductoFichaTecnica lgProductoFichaTecnica)
        {
            _context.LgProductoFichaTecnica.Add(lgProductoFichaTecnica);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLgProductoFichaTecnica", new { id = lgProductoFichaTecnica.ProductoFichaTecnicaId }, lgProductoFichaTecnica);
        }

        // DELETE: api/LgProductoFichaTecnicas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLgProductoFichaTecnica(int id)
        {
            var lgProductoFichaTecnica = await _context.LgProductoFichaTecnica.FindAsync(id);
            if (lgProductoFichaTecnica == null)
            {
                return NotFound();
            }

            _context.LgProductoFichaTecnica.Remove(lgProductoFichaTecnica);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LgProductoFichaTecnicaExists(int id)
        {
            return _context.LgProductoFichaTecnica.Any(e => e.ProductoFichaTecnicaId == id);
        }
    }
}
