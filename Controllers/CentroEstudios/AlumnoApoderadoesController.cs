using api_intiSoft.Models.CentroEstudios;
using intiSoft;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Controllers.CentroEstudios
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlumnoApoderadoesController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public AlumnoApoderadoesController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/AlumnoApoderadoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlumnoApoderado>>> GetAlumnoApoderado()
        {
            return await _context.AlumnoApoderado.ToListAsync();
        }

        // GET: api/AlumnoApoderadoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AlumnoApoderado>> GetAlumnoApoderado(int id)
        {
            var alumnoApoderado = await _context.AlumnoApoderado.FindAsync(id);

            if (alumnoApoderado == null)
            {
                return NotFound();
            }

            return alumnoApoderado;
        }

        //GET: AlumnoId




        // PUT: api/AlumnoApoderadoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAlumnoApoderado(int id, AlumnoApoderado alumnoApoderado)
        {
            if (id != alumnoApoderado.Id)
            {
                return BadRequest();
            }

            _context.Entry(alumnoApoderado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlumnoApoderadoExists(id))
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

        // POST: api/AlumnoApoderadoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AlumnoApoderado>> PostAlumnoApoderado(AlumnoApoderado alumnoApoderado)
        {
            _context.AlumnoApoderado.Add(alumnoApoderado);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAlumnoApoderado", new { id = alumnoApoderado.Id }, alumnoApoderado);
        }

        // DELETE: api/AlumnoApoderadoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlumnoApoderado(int id)
        {
            var alumnoApoderado = await _context.AlumnoApoderado.FindAsync(id);
            if (alumnoApoderado == null)
            {
                return NotFound();
            }

            _context.AlumnoApoderado.Remove(alumnoApoderado);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AlumnoApoderadoExists(int id)
        {
            return _context.AlumnoApoderado.Any(e => e.Id == id);
        }

        // GET: api/AlumnoApoderadoes/alumno/5
        [HttpGet("alumnoId/{alumnoId}")]
        public async Task<ActionResult<IEnumerable<AlumnoApoderado>>> GetAlumnoApoderadoByAlumnoId(int alumnoId)
        {
            var alumnoApoderados = await _context.AlumnoApoderado
                .Where(aa => aa.AlumnoId == alumnoId)
                .ToListAsync();

            return alumnoApoderados;
        }
    }
}
