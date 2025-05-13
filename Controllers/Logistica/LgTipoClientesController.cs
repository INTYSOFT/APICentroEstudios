using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_intiSoft.Models.Logistica;
using intiSoft;

namespace api_intiSoft.Controllers.Logistica
{
    [Route("api/[controller]")]
    [ApiController]
    public class LgTipoClientesController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public LgTipoClientesController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/LgTipoClientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LgTipoCliente>>> GetLgTipoCliente()
        {
            return await _context.LgTipoCliente.ToListAsync();
        }

        // GET: api/LgTipoClientes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LgTipoCliente>> GetLgTipoCliente(short id)
        {
            var lgTipoCliente = await _context.LgTipoCliente.FindAsync(id);

            if (lgTipoCliente == null)
            {
                return NotFound();
            }

            return lgTipoCliente;
        }

        // PUT: api/LgTipoClientes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLgTipoCliente(short id, LgTipoCliente lgTipoCliente)
        {
            if (id != lgTipoCliente.TipoClienteId)
            {
                return BadRequest();
            }

            _context.Entry(lgTipoCliente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LgTipoClienteExists(id))
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

        // POST: api/LgTipoClientes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LgTipoCliente>> PostLgTipoCliente(LgTipoCliente lgTipoCliente)
        {
            _context.LgTipoCliente.Add(lgTipoCliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLgTipoCliente", new { id = lgTipoCliente.TipoClienteId }, lgTipoCliente);
        }

        // DELETE: api/LgTipoClientes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLgTipoCliente(short id)
        {
            var lgTipoCliente = await _context.LgTipoCliente.FindAsync(id);
            if (lgTipoCliente == null)
            {
                return NotFound();
            }

            _context.LgTipoCliente.Remove(lgTipoCliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LgTipoClienteExists(short id)
        {
            return _context.LgTipoCliente.Any(e => e.TipoClienteId == id);
        }
    }
}
