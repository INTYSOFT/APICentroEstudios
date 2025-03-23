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
    public class LgUnidadMedidumsController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public LgUnidadMedidumsController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/LgUnidadMedidums
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LgUnidadMedidum>>> GetLgUnidadMedidum()
        {
            return await _context.LgUnidadMedidum.ToListAsync();
        }

        // GET: api/LgUnidadMedidums/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LgUnidadMedidum>> GetLgUnidadMedidum(int id)
        {
            var lgUnidadMedidum = await _context.LgUnidadMedidum.FindAsync(id);

            if (lgUnidadMedidum == null)
            {
                return NotFound();
            }

            return lgUnidadMedidum;
        }

        // PUT: api/LgUnidadMedidums/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLgUnidadMedidum(int id, LgUnidadMedidum lgUnidadMedidum)
        {
            if (id != lgUnidadMedidum.UnidadMedidaId)
            {
                return BadRequest();
            }

            _context.Entry(lgUnidadMedidum).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LgUnidadMedidumExists(id))
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

        // POST: api/LgUnidadMedidums
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LgUnidadMedidum>> PostLgUnidadMedidum(LgUnidadMedidum lgUnidadMedidum)
        {
            _context.LgUnidadMedidum.Add(lgUnidadMedidum);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLgUnidadMedidum", new { id = lgUnidadMedidum.UnidadMedidaId }, lgUnidadMedidum);
        }

        // DELETE: api/LgUnidadMedidums/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLgUnidadMedidum(int id)
        {
            var lgUnidadMedidum = await _context.LgUnidadMedidum.FindAsync(id);
            if (lgUnidadMedidum == null)
            {
                return NotFound();
            }

            _context.LgUnidadMedidum.Remove(lgUnidadMedidum);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LgUnidadMedidumExists(int id)
        {
            return _context.LgUnidadMedidum.Any(e => e.UnidadMedidaId == id);
        }
    }
}
