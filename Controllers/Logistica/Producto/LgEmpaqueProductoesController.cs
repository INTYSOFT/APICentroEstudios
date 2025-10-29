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
    public class LgEmpaqueProductoesController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public LgEmpaqueProductoesController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/LgEmpaqueProductoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LgEmpaqueProducto>>> GetLgEmpaqueProducto()
        {
            return await _context.LgEmpaqueProducto.ToListAsync();
        }

        // GET: api/LgEmpaqueProductoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LgEmpaqueProducto>> GetLgEmpaqueProducto(int id)
        {
            var lgEmpaqueProducto = await _context.LgEmpaqueProducto.FindAsync(id);

            if (lgEmpaqueProducto == null)
            {
                return NotFound();
            }

            return lgEmpaqueProducto;
        }

        // PUT: api/LgEmpaqueProductoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLgEmpaqueProducto(int id, LgEmpaqueProducto lgEmpaqueProducto)
        {
            if (id != lgEmpaqueProducto.EmpaqueProductoId)
            {
                return BadRequest();
            }

            _context.Entry(lgEmpaqueProducto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LgEmpaqueProductoExists(id))
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

        // POST: api/LgEmpaqueProductoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LgEmpaqueProducto>> PostLgEmpaqueProducto(LgEmpaqueProducto lgEmpaqueProducto)
        {
            _context.LgEmpaqueProducto.Add(lgEmpaqueProducto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLgEmpaqueProducto", new { id = lgEmpaqueProducto.EmpaqueProductoId }, lgEmpaqueProducto);
        }

        // DELETE: api/LgEmpaqueProductoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLgEmpaqueProducto(int id)
        {
            var lgEmpaqueProducto = await _context.LgEmpaqueProducto.FindAsync(id);
            if (lgEmpaqueProducto == null)
            {
                return NotFound();
            }

            _context.LgEmpaqueProducto.Remove(lgEmpaqueProducto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LgEmpaqueProductoExists(int id)
        {
            return _context.LgEmpaqueProducto.Any(e => e.EmpaqueProductoId == id);
        }
    }
}
