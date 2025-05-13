using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_intiSoft.Models.Clinica;
using intiSoft;

namespace api_intiSoft.Controllers.Citas
{
    [Route("api/[controller]")]
    [ApiController]
    public class GmEspecialidadsController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public GmEspecialidadsController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/GmEspecialidads
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GmEspecialidad>>> GetGmEspecialidad()
        {
            return await _context.GmEspecialidad.ToListAsync();
        }

        // GET: api/GmEspecialidads/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GmEspecialidad>> GetGmEspecialidad(int id)
        {
            var gmEspecialidad = await _context.GmEspecialidad.FindAsync(id);

            if (gmEspecialidad == null)
            {
                return NotFound();
            }

            return gmEspecialidad;
        }

        // PUT: api/GmEspecialidads/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGmEspecialidad(int id, GmEspecialidad gmEspecialidad)
        {
            if (id != gmEspecialidad.EspecialidadId)
            {
                return BadRequest();
            }

            _context.Entry(gmEspecialidad).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GmEspecialidadExists(id))
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

        // POST: api/GmEspecialidads
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GmEspecialidad>> PostGmEspecialidad(GmEspecialidad gmEspecialidad)
        {
            _context.GmEspecialidad.Add(gmEspecialidad);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGmEspecialidad", new { id = gmEspecialidad.EspecialidadId }, gmEspecialidad);
        }

        // DELETE: api/GmEspecialidads/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGmEspecialidad(int id)
        {
            var gmEspecialidad = await _context.GmEspecialidad.FindAsync(id);
            if (gmEspecialidad == null)
            {
                return NotFound();
            }

            _context.GmEspecialidad.Remove(gmEspecialidad);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GmEspecialidadExists(int id)
        {
            return _context.GmEspecialidad.Any(e => e.EspecialidadId == id);
        }
    }
}
