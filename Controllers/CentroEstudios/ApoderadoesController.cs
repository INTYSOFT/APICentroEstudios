using api_intiSoft.Models.CentroEstudios;
using intiSoft;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Controllers.CentroEstudios
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApoderadoesController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public ApoderadoesController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/Apoderadoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Apoderado>>> GetApoderado()
        {
            return await _context.Apoderado.ToListAsync();
        }

        // GET: api/Apoderadoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Apoderado>> GetApoderado(int id)
        {
            var apoderado = await _context.Apoderado.FindAsync(id);

            if (apoderado == null)
            {
                return NotFound();
            }

            return apoderado;
        }

        //Get por Documento, si no encuentra datos devuelve vacio.
        [HttpGet("documento/{documento}")]
        public async Task<ActionResult<Apoderado>> GetApoderadoPorDocumento(string documento)
        {
            var apoderado = await _context.Apoderado.Where(a => a.Documento == documento).FirstOrDefaultAsync();
            if (apoderado == null)
            {
                return NotFound();
            }
            return apoderado;
        }

        // PUT: api/Apoderadoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApoderado(int id, Apoderado apoderado)
        {
            if (id != apoderado.Id)
            {
                return BadRequest();
            }

            _context.Entry(apoderado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApoderadoExists(id))
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

        //Patch
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchApoderado(int id, Apoderado apoderado)
        {
            if (id != apoderado.Id)
            {
                return BadRequest();
            }
            _context.Entry(apoderado).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApoderadoExists(id))
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


        // POST: api/Apoderadoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Apoderado>> PostApoderado(Apoderado apoderado)
        {
            _context.Apoderado.Add(apoderado);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetApoderado", new { id = apoderado.Id }, apoderado);
        }

        // DELETE: api/Apoderadoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApoderado(int id)
        {
            var apoderado = await _context.Apoderado.FindAsync(id);
            if (apoderado == null)
            {
                return NotFound();
            }

            _context.Apoderado.Remove(apoderado);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ApoderadoExists(int id)
        {
            return _context.Apoderado.Any(e => e.Id == id);
        }
    }
}
