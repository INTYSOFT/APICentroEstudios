using api_intiSoft.Models.CentroEstudios;
using intiSoft;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Controllers.CentroEstudios
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluacionsController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public EvaluacionsController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/Evaluacions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Evaluacion>>> GetEvaluacion()
        {
            return await _context.Evaluacion.ToListAsync();
        }

        // GET: api/Evaluacions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Evaluacion>> GetEvaluacion(int id)
        {
            var evaluacion = await _context.Evaluacion.FindAsync(id);

            if (evaluacion == null)
            {
                return new ActionResult<Evaluacion>(new Evaluacion());
            }

            return evaluacion;
        }

        //Get. por sedeId
        [HttpGet("sede/{sedeId}")]
        public async Task<ActionResult<IEnumerable<Evaluacion>>> GetEvaluacionBySede(int sedeId)
        {
            var evaluacions = await _context.Evaluacion
                .Where(e => e.SedeId == sedeId)
                .ToListAsync();
            if (evaluacions == null || evaluacions.Count == 0)
            {
                return new List<Evaluacion>();
            }
            return evaluacions;
        }

        //GET: por sedeId y cicloId.
        [HttpGet("sedes/{sedeId}/ciclo/{cicloId}")]
        public async Task<ActionResult<IEnumerable<Evaluacion>>> GetEvaluacionBySedeAndCiclo(int sedeId, int cicloId)
        {
            var evaluacions = await _context.Evaluacion
                .Where(e => e.SedeId == sedeId && e.CicloId == cicloId)
                .ToListAsync();
            if (evaluacions == null || evaluacions.Count == 0)
            {
                return new List<Evaluacion>();
            }
            return evaluacions;
        }


        //GET por sedeId, cicloId y seccionId
        [HttpGet("sede/{sedeId}/ciclos/{cicloId}/seccion/{seccionId}")]
        public async Task<ActionResult<IEnumerable<Evaluacion>>> GetEvaluacionBySedeCicloSeccion(int sedeId, int cicloId, int seccionId)
        {
            var evaluacions = await _context.Evaluacion
                .Where(e => e.SedeId == sedeId && e.CicloId == cicloId && e.SeccionId == seccionId)
                .ToListAsync();
            if (evaluacions == null || evaluacions.Count == 0)
            {
                return new List<Evaluacion>();
            }
            return evaluacions;
        }

        //GET por alumnoId
        [HttpGet("alumno/{alumnoId}")]
        public async Task<ActionResult<IEnumerable<Evaluacion>>> GetEvaluacionByAlumnoId(int alumnoId)
        {
            var evaluacions = await _context.Evaluacion
                .Where(e => e.AlumnoId == alumnoId)
                .ToListAsync();
            if (evaluacions == null || evaluacions.Count == 0)
            {
                return new List<Evaluacion>();
            }
            return evaluacions;
        }

        //Get por evaluacionProgramadaId
        [HttpGet("evaluacionProgramada/{evaluacionProgramadaId}")]
        public async Task<ActionResult<IEnumerable<Evaluacion>>> GetEvaluacionByEvaluacionProgramadaId(int evaluacionProgramadaId)
        {
            var evaluacions = await _context.Evaluacion
                .Where(e => e.EvaluacionProgramadaId == evaluacionProgramadaId)
                .ToListAsync();
            if (evaluacions == null || evaluacions.Count == 0)
            {
                return new List<Evaluacion>();
            }
            return evaluacions;
        }


        //Get por evaluacionProgramadaId y alumnoId
        [HttpGet("evaluacionProgramada/{evaluacionProgramadaId}/alumno/{alumnoId}")]
        public async Task<ActionResult<IEnumerable<Evaluacion>>> GetEvaluacionByEvaluacionProgramadaAndAlumno(int evaluacionProgramadaId, int alumnoId)
        {
            var evaluacions = await _context.Evaluacion
                .Where(e => e.EvaluacionProgramadaId == evaluacionProgramadaId && e.AlumnoId == alumnoId)
                .ToListAsync();
            if (evaluacions == null || evaluacions.Count == 0)
            {
                return new List<Evaluacion>();
            }
            return evaluacions;
        }

        // PUT: api/Evaluacions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvaluacion(int id, Evaluacion evaluacion)
        {
            if (id != evaluacion.Id)
            {
                return BadRequest();
            }

            _context.Entry(evaluacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EvaluacionExists(id))
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

        //buscar por SedeID, CicloId y AlumnoId
        [HttpGet("sede/{sedeId}/ciclo/{cicloId}/alumno/{alumnoId}")]
        public async Task<ActionResult<IEnumerable<Evaluacion>>> GetEvaluacionBySedeCicloAlumno(int sedeId, int cicloId, int alumnoId)
        {
            var evaluacions = await _context.Evaluacion
                .Where(e => e.SedeId == sedeId && e.CicloId == cicloId && e.AlumnoId == alumnoId)
                .ToListAsync();
            if (evaluacions == null || evaluacions.Count == 0)
            {
                return new List<Evaluacion>();
            }
            return evaluacions;
        }

        // GET: api/Evaluacions/porAlumno/5
        [HttpGet("porAlumno/{alumnoId}")]
        public async Task<ActionResult<IEnumerable<Evaluacion>>> GetEvaluacionByAlumno(int alumnoId)
        {
            var evaluacions = await _context.Evaluacion
                .Where(e => e.AlumnoId == alumnoId)
                .ToListAsync();
            if (evaluacions == null || evaluacions.Count == 0)
            {
                return new List<Evaluacion>();
            }
            return evaluacions;
        }

        //get por sedeid, cicloid, seccionid
        [HttpGet("sede/{sedeId}/ciclo/{cicloId}/seccion/{seccionId}")]
        public async Task<ActionResult<IEnumerable<Evaluacion>>> GetEvaluacionBySedeCicloSecciones(int sedeId, int cicloId, int seccionId)
        {
            var evaluacions = await _context.Evaluacion
                .Where(e => e.SedeId == sedeId && e.CicloId == cicloId && e.SeccionId == seccionId)
                .ToListAsync();
            if (evaluacions == null || evaluacions.Count == 0)
            {
                return new List<Evaluacion>();
            }
            return evaluacions;
        }

        //Get por sedeId y cicloId
        [HttpGet("sede/{sedeId}/ciclo/{cicloId}")]
        public async Task<ActionResult<IEnumerable<Evaluacion>>> GetEvaluacionBySedeCiclo(int sedeId, int cicloId)
        {
            var evaluacions = await _context.Evaluacion
                .Where(e => e.SedeId == sedeId && e.CicloId == cicloId)
                .ToListAsync();
            if (evaluacions == null || evaluacions.Count == 0)
            {
                return new List<Evaluacion>();
            }
            return evaluacions;
        }

        //GET: por evaluacionProgramadaId
        [HttpGet("evaluacionProgramadaId/{evaluacionProgramadaId}")]
        public async Task<ActionResult<IEnumerable<Evaluacion>>> GetEvaluacionByEvaluacionProgramadaIdAlt(int evaluacionProgramadaId)
        {
            var evaluacions = await _context.Evaluacion
                .Where(e => e.EvaluacionProgramadaId == evaluacionProgramadaId)
                .ToListAsync();
            if (evaluacions == null || evaluacions.Count == 0)
            {
                return new List<Evaluacion>();
            }
            return evaluacions;
        }




        // GET: api/Evaluacions/porEvaluacionProgramada/5
        [HttpGet("porEvaluacionProgramada/{evaluacionProgramadaId}")]
        public async Task<ActionResult<IEnumerable<Evaluacion>>> GetEvaluacionByEvaluacionProgramada(int evaluacionProgramadaId)
        {
            var evaluacions = await _context.Evaluacion
                .Where(e => e.EvaluacionProgramadaId == evaluacionProgramadaId)
                .ToListAsync();
            if (evaluacions == null || evaluacions.Count == 0)
            {
                return new List<Evaluacion>();
            }
            return evaluacions;
        }


        // POST: api/Evaluacions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Evaluacion>> PostEvaluacion(Evaluacion evaluacion)
        {
            _context.Evaluacion.Add(evaluacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvaluacion", new { id = evaluacion.Id }, evaluacion);
        }

        // DELETE: api/Evaluacions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvaluacion(int id)
        {
            var evaluacion = await _context.Evaluacion.FindAsync(id);
            if (evaluacion == null)
            {
                return NotFound();
            }

            _context.Evaluacion.Remove(evaluacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EvaluacionExists(int id)
        {
            return _context.Evaluacion.Any(e => e.Id == id);
        }
    }
}
