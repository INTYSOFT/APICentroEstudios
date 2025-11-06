using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_intiSoft.Models.CentroEstudios;
using intiSoft;

namespace api_intiSoft.Controllers.CentroEstudios
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadoEvaluacionProgramadumsController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public EstadoEvaluacionProgramadumsController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/EstadoEvaluacionProgramadums
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstadoEvaluacionProgramadum>>> GetEstadoEvaluacionProgramadum()
        {
            return await _context.EstadoEvaluacionProgramadum.ToListAsync();
        }

        // GET: api/EstadoEvaluacionProgramadums/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EstadoEvaluacionProgramadum>> GetEstadoEvaluacionProgramadum(int id)
        {
            var estadoEvaluacionProgramadum = await _context.EstadoEvaluacionProgramadum.FindAsync(id);

            if (estadoEvaluacionProgramadum == null)
            {
                return NotFound();
            }

            return estadoEvaluacionProgramadum;
        }

        // PUT: api/EstadoEvaluacionProgramadums/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstadoEvaluacionProgramadum(int id, EstadoEvaluacionProgramadum estadoEvaluacionProgramadum)
        {
            if (id != estadoEvaluacionProgramadum.Id)
            {
                return BadRequest();
            }

            _context.Entry(estadoEvaluacionProgramadum).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstadoEvaluacionProgramadumExists(id))
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

        // POST: api/EstadoEvaluacionProgramadums
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EstadoEvaluacionProgramadum>> PostEstadoEvaluacionProgramadum(EstadoEvaluacionProgramadum estadoEvaluacionProgramadum)
        {
            _context.EstadoEvaluacionProgramadum.Add(estadoEvaluacionProgramadum);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEstadoEvaluacionProgramadum", new { id = estadoEvaluacionProgramadum.Id }, estadoEvaluacionProgramadum);
        }

        // DELETE: api/EstadoEvaluacionProgramadums/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstadoEvaluacionProgramadum(int id)
        {
            var estadoEvaluacionProgramadum = await _context.EstadoEvaluacionProgramadum.FindAsync(id);
            if (estadoEvaluacionProgramadum == null)
            {
                return NotFound();
            }

            _context.EstadoEvaluacionProgramadum.Remove(estadoEvaluacionProgramadum);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EstadoEvaluacionProgramadumExists(int id)
        {
            return _context.EstadoEvaluacionProgramadum.Any(e => e.Id == id);
        }
    }
}
