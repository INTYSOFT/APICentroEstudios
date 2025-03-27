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
    public class LgModeloesController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public LgModeloesController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/LgModeloes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LgModelo>>> GetLgModelo()
        {
            return await _context.LgModelo.ToListAsync();
        }

        // GET: api/LgModeloes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LgModelo>> GetLgModelo(int id)
        {
            var lgModelo = await _context.LgModelo.FindAsync(id);

            if (lgModelo == null)
            {
                return NotFound();
            }

            return lgModelo;
        }

        // PUT: api/LgModeloes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLgModelo(int id, LgModelo lgModelo)
        {
            if (id != lgModelo.ModeloId)
            {
                return BadRequest();
            }

            _context.Entry(lgModelo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LgModeloExists(id))
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

        // POST: api/LgModeloes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LgModelo>> PostLgModelo(LgModelo lgModelo)
        {
            _context.LgModelo.Add(lgModelo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLgModelo", new { id = lgModelo.ModeloId }, lgModelo);
        }

        // DELETE: api/LgModeloes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLgModelo(int id)
        {
            var lgModelo = await _context.LgModelo.FindAsync(id);
            if (lgModelo == null)
            {
                return NotFound();
            }

            _context.LgModelo.Remove(lgModelo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LgModeloExists(int id)
        {
            return _context.LgModelo.Any(e => e.ModeloId == id);
        }
    }
}
