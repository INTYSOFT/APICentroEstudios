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
    public class LgTipoTransaccionsController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public LgTipoTransaccionsController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/LgTipoTransaccions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LgTipoTransaccion>>> GetLgTipoTransaccion()
        {
            return await _context.LgTipoTransaccion.ToListAsync();
        }

        // GET: api/LgTipoTransaccions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LgTipoTransaccion>> GetLgTipoTransaccion(short id)
        {
            var lgTipoTransaccion = await _context.LgTipoTransaccion.FindAsync(id);

            if (lgTipoTransaccion == null)
            {
                return NotFound();
            }

            return lgTipoTransaccion;
        }

        // PUT: api/LgTipoTransaccions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLgTipoTransaccion(short id, LgTipoTransaccion lgTipoTransaccion)
        {
            if (id != lgTipoTransaccion.TipoTransaccionId)
            {
                return BadRequest();
            }

            _context.Entry(lgTipoTransaccion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LgTipoTransaccionExists(id))
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

        // POST: api/LgTipoTransaccions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LgTipoTransaccion>> PostLgTipoTransaccion(LgTipoTransaccion lgTipoTransaccion)
        {
            _context.LgTipoTransaccion.Add(lgTipoTransaccion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLgTipoTransaccion", new { id = lgTipoTransaccion.TipoTransaccionId }, lgTipoTransaccion);
        }

        // DELETE: api/LgTipoTransaccions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLgTipoTransaccion(short id)
        {
            var lgTipoTransaccion = await _context.LgTipoTransaccion.FindAsync(id);
            if (lgTipoTransaccion == null)
            {
                return NotFound();
            }

            _context.LgTipoTransaccion.Remove(lgTipoTransaccion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LgTipoTransaccionExists(short id)
        {
            return _context.LgTipoTransaccion.Any(e => e.TipoTransaccionId == id);
        }
    }
}
