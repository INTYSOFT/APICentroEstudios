using api_intiSoft.Models.CentroEstudios;
using intiSoft;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Controllers.CentroEstudios
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluacionClavesController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public EvaluacionClavesController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/EvaluacionClaves
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EvaluacionClave>>> GetEvaluacionClave()
        {
            return await _context.EvaluacionClave.ToListAsync();
        }

        // GET: api/EvaluacionClaves/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EvaluacionClave>> GetEvaluacionClave(int id)
        {
            var evaluacionClave = await _context.EvaluacionClave.FindAsync(id);

            if (evaluacionClave == null)
            {
                return NotFound();
            }

            return evaluacionClave;
        }

        //Get por evaluacion_detalle_id
        [HttpGet("evaluacion_detalle/{evaluacion_detalle_id}")]
        public async Task<ActionResult<IEnumerable<EvaluacionClave>>> GetEvaluacionClaveByEvaluacionDetalleId(int evaluacion_detalle_id)
        {
            var evaluacionClaves = await _context.EvaluacionClave
                .Where(ec => ec.EvaluacionDetalleId == evaluacion_detalle_id)
                .ToListAsync();
            if (evaluacionClaves == null || evaluacionClaves.Count == 0)
            {
                return NotFound();
            }
            return evaluacionClaves;
        }

        // PUT: api/EvaluacionClaves/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvaluacionClave(int id, EvaluacionClave evaluacionClave)
        {
            if (id != evaluacionClave.Id)
            {
                return BadRequest();
            }

            _context.Entry(evaluacionClave).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EvaluacionClaveExists(id))
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

        // POST: api/EvaluacionClaves
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EvaluacionClave>> PostEvaluacionClave(EvaluacionClave evaluacionClave)
        {
            _context.EvaluacionClave.Add(evaluacionClave);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvaluacionClave", new { id = evaluacionClave.Id }, evaluacionClave);
        }

        // DELETE: api/EvaluacionClaves/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvaluacionClave(int id)
        {
            var evaluacionClave = await _context.EvaluacionClave.FindAsync(id);
            if (evaluacionClave == null)
            {
                return NotFound();
            }

            _context.EvaluacionClave.Remove(evaluacionClave);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EvaluacionClaveExists(int id)
        {
            return _context.EvaluacionClave.Any(e => e.Id == id);
        }
    }
}
