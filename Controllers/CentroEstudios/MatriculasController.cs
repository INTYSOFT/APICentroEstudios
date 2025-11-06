using api_intiSoft.Models.CentroEstudios;
using intiSoft;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Controllers.CentroEstudios
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatriculasController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public MatriculasController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/Matriculas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Matricula>>> GetMatricula()
        {
            return await _context.Matricula.ToListAsync();
        }

        // GET: api/Matriculas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Matricula>> GetMatricula(int id)
        {
            var matricula = await _context.Matricula.FindAsync(id);

            if (matricula == null)
            {
                return new ActionResult<Matricula>(NotFound());
            }

            return matricula;
        }

        //GET: api/AlumnoId/SeccionCicloId
        [HttpGet("GetMatriculaByAlumnoSeccion/{alumnoId}/{seccionCicloId}")]
        public async Task<ActionResult<Matricula>> GetMatriculaByAlumnoSeccion(int alumnoId, int seccionCicloId)
        {
            var matricula = await _context.Matricula
                .Where(m => m.AlumnoId == alumnoId && m.SeccionCicloId == seccionCicloId)
                .FirstOrDefaultAsync();
            if (matricula == null)
            {
                return new ActionResult<Matricula>(NotFound());
            }
            return matricula;
        }

        // GET: api/AlumnoId/CicloId
        [HttpGet("GetMatriculasByAlumnoCiclo/{alumnoId}/{cicloId}")]
        public async Task<ActionResult<IEnumerable<Matricula>>> GetMatriculasByAlumnoCiclo(int alumnoId, int cicloId)
        {
            var matriculas = await _context.Matricula
                .Where(m => m.AlumnoId == alumnoId && m.CicloId == cicloId)
                .ToListAsync();
            if (matriculas == null || matriculas.Count == 0)
            {
                // Return an empty list instead of NotFound
                return new List<Matricula>();
            }
            return matriculas;
        }

        //Get seccion y ciclo
        [HttpGet("GetMatriculasBySeccionCiclo/{seccionCicloId}")]
        public async Task<ActionResult<IEnumerable<Matricula>>> GetMatriculasBySeccionCiclo(int seccionCicloId)
        {
            var matriculas = await _context.Matricula
                .Where(m => m.SeccionCicloId == seccionCicloId)
                .ToListAsync();
            if (matriculas == null || matriculas.Count == 0)
            {
                // Return an empty list instead of NotFound
                return new List<Matricula>();
            }
            return matriculas;
        }

        //Get sede , ciclo, seccion
        [HttpGet("GetMatriculasBySedeCicloSeccion/{sedeId}/{cicloId}/{seccionId}")]
        public async Task<ActionResult<IEnumerable<Matricula>>> GetMatriculasBySedeCicloSeccion(int sedeId, int cicloId, int seccionId)
        {
            var matriculas = await _context.Matricula
                .Where(m => m.SedeId == sedeId && m.CicloId == cicloId && m.SeccionId == seccionId)
                .ToListAsync();
            if (matriculas == null || matriculas.Count == 0)
            {
                // Return an empty list instead of NotFound
                return new List<Matricula>();
            }
            return matriculas;
        }

        //Gewt por Sede, Ciclo
        [HttpGet("GetMatriculasBySedeCiclo/{sedeId}/{cicloId}")]
        public async Task<ActionResult<IEnumerable<Matricula>>> GetMatriculasBySedeCiclo(int sedeId, int cicloId)
        {
            var matriculas = await _context.Matricula
                .Where(m => m.SedeId == sedeId && m.CicloId == cicloId)
                .ToListAsync();
            if (matriculas == null || matriculas.Count == 0)
            {
                // Return an empty list instead of NotFound
                return new List<Matricula>();
            }
            return matriculas;
        }


        // PUT: api/Matriculas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMatricula(int id, Matricula matricula)
        {
            if (id != matricula.Id)
            {
                return BadRequest();
            }

            _context.Entry(matricula).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MatriculaExists(id))
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

        // POST: api/Matriculas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Matricula>> PostMatricula(Matricula matricula)
        {
            _context.Matricula.Add(matricula);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMatricula", new { id = matricula.Id }, matricula);
        }

        // DELETE: api/Matriculas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatricula(int id)
        {
            var matricula = await _context.Matricula.FindAsync(id);
            if (matricula == null)
            {
                return NotFound();
            }

            _context.Matricula.Remove(matricula);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MatriculaExists(int id)
        {
            return _context.Matricula.Any(e => e.Id == id);
        }
    }
}
