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
    public class LgTipoMarcasController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public LgTipoMarcasController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/LgTipoMarcas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LgTipoMarca>>> GetLgTipoMarca()
        {
            return await _context.LgTipoMarca.ToListAsync();
        }

        // GET: api/LgTipoMarcas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LgTipoMarca>> GetLgTipoMarca(int id)
        {
            var lgTipoMarca = await _context.LgTipoMarca.FindAsync(id);

            if (lgTipoMarca == null)
            {
                return NotFound();
            }

            return lgTipoMarca;
        }

        // PUT: api/LgTipoMarcas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLgTipoMarca(int id, LgTipoMarca lgTipoMarca)
        {
            if (id != lgTipoMarca.TipoMarcaId)
            {
                return BadRequest();
            }

            _context.Entry(lgTipoMarca).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LgTipoMarcaExists(id))
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

        // POST: api/LgTipoMarcas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LgTipoMarca>> PostLgTipoMarca(LgTipoMarca lgTipoMarca)
        {
            _context.LgTipoMarca.Add(lgTipoMarca);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLgTipoMarca", new { id = lgTipoMarca.TipoMarcaId }, lgTipoMarca);
        }

        // DELETE: api/LgTipoMarcas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLgTipoMarca(int id)
        {
            var lgTipoMarca = await _context.LgTipoMarca.FindAsync(id);
            if (lgTipoMarca == null)
            {
                return NotFound();
            }

            _context.LgTipoMarca.Remove(lgTipoMarca);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LgTipoMarcaExists(int id)
        {
            return _context.LgTipoMarca.Any(e => e.TipoMarcaId == id);
        }
    }
}
