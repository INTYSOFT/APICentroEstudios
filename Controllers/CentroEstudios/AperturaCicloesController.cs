using api_intiSoft.Models.CentroEstudios;
using intiSoft;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Controllers.CentroEstudios
{
    [Route("api/[controller]")]
    [ApiController]
    public class AperturaCicloesController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public AperturaCicloesController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/AperturaCicloes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AperturaCiclo>>> GetAperturaCiclo()
        {
            return await _context.AperturaCiclo.ToListAsync();
        }

        // GET: api/AperturaCicloes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AperturaCiclo>> GetAperturaCiclo(int id)
        {
            var aperturaCiclo = await _context.AperturaCiclo.FindAsync(id);

            if (aperturaCiclo == null)
            {
                //return NotFound();
                return new AperturaCiclo();
            }

            return aperturaCiclo;
        }

        //GET por ciclo, devuelve array cuando no exista datos.
        [HttpGet("ciclo/{cicloId}")]
        public async Task<ActionResult<IEnumerable<AperturaCiclo>>> GetAperturaCicloByCiclo(int cicloId)
        {
            var aperturaCiclos = await _context.AperturaCiclo
                .Where(ac => ac.CicloId == cicloId)
                .ToListAsync();
            if (aperturaCiclos == null || aperturaCiclos.Count == 0)
            {
                //return vacio
                return new List<AperturaCiclo>();
            }
            return aperturaCiclos;
        }


        //GET por sede
        [HttpGet("sede/{sedeId}")]
        public async Task<ActionResult<IEnumerable<AperturaCiclo>>> GetAperturaCicloBySede(int sedeId)
        {
            var aperturaCiclos = await _context.AperturaCiclo
                .Where(ac => ac.SedeId == sedeId)
                .ToListAsync();
            if (aperturaCiclos == null || aperturaCiclos.Count == 0)
            {
                //return vacio
                return new List<AperturaCiclo>();
            }
            return aperturaCiclos;
        }


        // PUT: api/AperturaCicloes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAperturaCiclo(int id, AperturaCiclo aperturaCiclo)
        {
            if (id != aperturaCiclo.Id)
            {
                return BadRequest();
            }

            _context.Entry(aperturaCiclo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AperturaCicloExists(id))
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

        // POST: api/AperturaCicloes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AperturaCiclo>> PostAperturaCiclo(AperturaCiclo aperturaCiclo)
        {
            _context.AperturaCiclo.Add(aperturaCiclo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAperturaCiclo", new { id = aperturaCiclo.Id }, aperturaCiclo);
        }

        //PATCH
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchAperturaCiclo(int id, AperturaCiclo aperturaCiclo)
        {
            if (id != aperturaCiclo.Id)
            {
                return BadRequest();
            }
            _context.Entry(aperturaCiclo).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AperturaCicloExists(id))
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

        // DELETE: api/AperturaCicloes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAperturaCiclo(int id)
        {
            var aperturaCiclo = await _context.AperturaCiclo.FindAsync(id);
            if (aperturaCiclo == null)
            {
                return NotFound();
            }

            _context.AperturaCiclo.Remove(aperturaCiclo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AperturaCicloExists(int id)
        {
            return _context.AperturaCiclo.Any(e => e.Id == id);
        }
    }
}
