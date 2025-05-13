using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_intiSoft.Models.Clinica;
using intiSoft;

namespace api_intiSoft.Controllers.Clinica
{
    [Route("api/[controller]")]
    [ApiController]
    public class GmTipoHorariosController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public GmTipoHorariosController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/GmTipoHorarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GmTipoHorario>>> GetGmTipoHorario()
        {
            return await _context.GmTipoHorario.ToListAsync();
        }

        // GET: api/GmTipoHorarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GmTipoHorario>> GetGmTipoHorario(short id)
        {
            var gmTipoHorario = await _context.GmTipoHorario.FindAsync(id);

            if (gmTipoHorario == null)
            {
                return NotFound();
            }

            return gmTipoHorario;
        }

        // PUT: api/GmTipoHorarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGmTipoHorario(short id, GmTipoHorario gmTipoHorario)
        {
            if (id != gmTipoHorario.TipoHorarioId)
            {
                return BadRequest();
            }

            _context.Entry(gmTipoHorario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GmTipoHorarioExists(id))
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

        // POST: api/GmTipoHorarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GmTipoHorario>> PostGmTipoHorario(GmTipoHorario gmTipoHorario)
        {
            _context.GmTipoHorario.Add(gmTipoHorario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGmTipoHorario", new { id = gmTipoHorario.TipoHorarioId }, gmTipoHorario);
        }

        // DELETE: api/GmTipoHorarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGmTipoHorario(short id)
        {
            var gmTipoHorario = await _context.GmTipoHorario.FindAsync(id);
            if (gmTipoHorario == null)
            {
                return NotFound();
            }

            _context.GmTipoHorario.Remove(gmTipoHorario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GmTipoHorarioExists(short id)
        {
            return _context.GmTipoHorario.Any(e => e.TipoHorarioId == id);
        }
    }
}
