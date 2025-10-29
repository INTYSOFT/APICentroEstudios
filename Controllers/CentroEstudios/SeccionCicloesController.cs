using api_intiSoft.Models.CentroEstudios;
using intiSoft;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Controllers.CentroEstudios
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeccionCicloesController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public SeccionCicloesController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/SeccionCicloes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SeccionCiclo>>> GetSeccionCiclo()
        {
            return await _context.SeccionCiclo.ToListAsync();
        }

        // GET: api/SeccionCicloes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SeccionCiclo>> GetSeccionCiclo(int id)
        {
            var seccionCiclo = await _context.SeccionCiclo.FindAsync(id);

            if (seccionCiclo == null)
            {
                return NotFound();
            }

            return seccionCiclo;
        }

        //get por ciclo_id
        [HttpGet("ciclo/{ciclo_id}")]
        public async Task<ActionResult<IEnumerable<SeccionCiclo>>> GetSeccionCicloByCiclo(int ciclo_id)
        {
            var seccionCiclos = await _context.SeccionCiclo.Where(sc => sc.CicloId == ciclo_id).ToListAsync();
            if (seccionCiclos == null || seccionCiclos.Count == 0)
            {
                // return vacio
                return new List<SeccionCiclo>();
            }
            return seccionCiclos;
        }

        //get por sede
        [HttpGet("sede/{sede_id}")]
        public async Task<ActionResult<IEnumerable<SeccionCiclo>>> GetSeccionCicloBySede(int sede_id)
        {
            var seccionCiclos = await _context.SeccionCiclo.Where(sc => sc.SedeId == sede_id).ToListAsync();
            if (seccionCiclos == null || seccionCiclos.Count == 0)
            {
                // return vacio
                return new List<SeccionCiclo>();
            }
            return seccionCiclos;
        }

        //get por sede y ciclo
        [HttpGet("sede/{sede_id}/ciclo/{ciclo_id}")]
        public async Task<ActionResult<IEnumerable<SeccionCiclo>>> GetSeccionCicloBySedeAndCiclo(int sede_id, int ciclo_id)
        {
            var seccionCiclos = await _context.SeccionCiclo.Where(sc => sc.SedeId == sede_id && sc.CicloId == ciclo_id).ToListAsync();
            if (seccionCiclos == null || seccionCiclos.Count == 0)
            {
                // return vacio
                return new List<SeccionCiclo>();
            }
            return seccionCiclos;
        }


        // PUT: api/SeccionCicloes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSeccionCiclo(int id, SeccionCiclo seccionCiclo)
        {
            if (id != seccionCiclo.Id)
            {
                return BadRequest();
            }

            _context.Entry(seccionCiclo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SeccionCicloExists(id))
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

        // POST: api/SeccionCicloes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SeccionCiclo>> PostSeccionCiclo(SeccionCiclo seccionCiclo)
        {
            _context.SeccionCiclo.Add(seccionCiclo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSeccionCiclo", new { id = seccionCiclo.Id }, seccionCiclo);
        }

        //patch
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchSeccionCiclo(int id, SeccionCiclo seccionCiclo)
        {
            if (id != seccionCiclo.Id)
            {
                return BadRequest();
            }
            _context.Entry(seccionCiclo).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SeccionCicloExists(id))
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

        // DELETE: api/SeccionCicloes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeccionCiclo(int id)
        {
            var seccionCiclo = await _context.SeccionCiclo.FindAsync(id);
            if (seccionCiclo == null)
            {
                return NotFound();
            }

            _context.SeccionCiclo.Remove(seccionCiclo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SeccionCicloExists(int id)
        {
            return _context.SeccionCiclo.Any(e => e.Id == id);
        }
    }
}
