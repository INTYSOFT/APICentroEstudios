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
    public class LgEmpaquesController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public LgEmpaquesController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/LgEmpaques
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LgEmpaque>>> GetLgEmpaque()
        {
            return await _context.LgEmpaque.ToListAsync();
        }

        // GET: api/LgEmpaques/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LgEmpaque>> GetLgEmpaque(int id)
        {
            var lgEmpaque = await _context.LgEmpaque.FindAsync(id);

            if (lgEmpaque == null)
            {
                return NotFound();
            }

            return lgEmpaque;
        }

        // PUT: api/LgEmpaques/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLgEmpaque(int id, LgEmpaque lgEmpaque)
        {
            if (id != lgEmpaque.EmpaqueId)
            {
                return BadRequest();
            }

            _context.Entry(lgEmpaque).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LgEmpaqueExists(id))
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

        // POST: api/LgEmpaques
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LgEmpaque>> PostLgEmpaque(LgEmpaque lgEmpaque)
        {
            _context.LgEmpaque.Add(lgEmpaque);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLgEmpaque", new { id = lgEmpaque.EmpaqueId }, lgEmpaque);
        }

        // DELETE: api/LgEmpaques/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLgEmpaque(int id)
        {
            var lgEmpaque = await _context.LgEmpaque.FindAsync(id);
            if (lgEmpaque == null)
            {
                return NotFound();
            }

            _context.LgEmpaque.Remove(lgEmpaque);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LgEmpaqueExists(int id)
        {
            return _context.LgEmpaque.Any(e => e.EmpaqueId == id);
        }
    }
}
