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
    public class TipoListumsController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public TipoListumsController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/LgTipoListums
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoListum>>> GetLgTipoListum()
        {
            return await _context.LgTipoListum.ToListAsync();
        }

        // GET: api/LgTipoListums/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoListum>> GetLgTipoListum(short id)
        {
            var lgTipoListum = await _context.LgTipoListum.FindAsync(id);

            if (lgTipoListum == null)
            {
                return NotFound();
            }

            return lgTipoListum;
        }

        // PUT: api/LgTipoListums/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLgTipoListum(short id, TipoListum lgTipoListum)
        {
            if (id != lgTipoListum.TipoListaId)
            {
                return BadRequest();
            }

            _context.Entry(lgTipoListum).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LgTipoListumExists(id))
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

        // POST: api/LgTipoListums
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TipoListum>> PostLgTipoListum(TipoListum lgTipoListum)
        {
            _context.LgTipoListum.Add(lgTipoListum);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLgTipoListum", new { id = lgTipoListum.TipoListaId }, lgTipoListum);
        }

        // DELETE: api/LgTipoListums/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLgTipoListum(short id)
        {
            var lgTipoListum = await _context.LgTipoListum.FindAsync(id);
            if (lgTipoListum == null)
            {
                return NotFound();
            }

            _context.LgTipoListum.Remove(lgTipoListum);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LgTipoListumExists(short id)
        {
            return _context.LgTipoListum.Any(e => e.TipoListaId == id);
        }
    }
}
