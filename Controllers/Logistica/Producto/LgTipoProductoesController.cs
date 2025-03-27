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
    public class LgTipoProductoesController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public LgTipoProductoesController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/LgTipoProductoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LgTipoProducto>>> GetLgTipoProducto()
        {
            return await _context.LgTipoProducto.ToListAsync();
        }

        // GET: api/LgTipoProductoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LgTipoProducto>> GetLgTipoProducto(short id)
        {
            var lgTipoProducto = await _context.LgTipoProducto.FindAsync(id);

            if (lgTipoProducto == null)
            {
                return NotFound();
            }

            return lgTipoProducto;
        }

        // PUT: api/LgTipoProductoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLgTipoProducto(short id, LgTipoProducto lgTipoProducto)
        {
            if (id != lgTipoProducto.TipoProductoId)
            {
                return BadRequest();
            }

            _context.Entry(lgTipoProducto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LgTipoProductoExists(id))
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

        // POST: api/LgTipoProductoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LgTipoProducto>> PostLgTipoProducto(LgTipoProducto lgTipoProducto)
        {
            _context.LgTipoProducto.Add(lgTipoProducto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLgTipoProducto", new { id = lgTipoProducto.TipoProductoId }, lgTipoProducto);
        }

        // DELETE: api/LgTipoProductoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLgTipoProducto(short id)
        {
            var lgTipoProducto = await _context.LgTipoProducto.FindAsync(id);
            if (lgTipoProducto == null)
            {
                return NotFound();
            }

            _context.LgTipoProducto.Remove(lgTipoProducto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LgTipoProductoExists(short id)
        {
            return _context.LgTipoProducto.Any(e => e.TipoProductoId == id);
        }
    }
}
