using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_intiSoft.Models.Seguridad;
using intiSoft;

namespace api_intiSoft.Controllers.Seguridad
{
    [Route("api/[controller]")]
    [ApiController]
    public class SgUsuariosController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public SgUsuariosController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/SgUsuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SgUsuario>>> GetSgUsuario()
        {
            return await _context.SgUsuario.ToListAsync();
        }

        // GET: api/SgUsuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SgUsuario>> GetSgUsuario(int id)
        {
            var sgUsuario = await _context.SgUsuario.FindAsync(id);

            if (sgUsuario == null)
            {
                return NotFound();
            }

            return sgUsuario;
        }

        // PUT: api/SgUsuarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSgUsuario(int id, SgUsuario sgUsuario)
        {
            if (id != sgUsuario.UsuarioId)
            {
                return BadRequest();
            }

            _context.Entry(sgUsuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SgUsuarioExists(id))
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

        // POST: api/SgUsuarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SgUsuario>> PostSgUsuario(SgUsuario sgUsuario)
        {
            _context.SgUsuario.Add(sgUsuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSgUsuario", new { id = sgUsuario.UsuarioId }, sgUsuario);
        }

        // DELETE: api/SgUsuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSgUsuario(int id)
        {
            var sgUsuario = await _context.SgUsuario.FindAsync(id);
            if (sgUsuario == null)
            {
                return NotFound();
            }

            _context.SgUsuario.Remove(sgUsuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SgUsuarioExists(int id)
        {
            return _context.SgUsuario.Any(e => e.UsuarioId == id);
        }
    }
}
