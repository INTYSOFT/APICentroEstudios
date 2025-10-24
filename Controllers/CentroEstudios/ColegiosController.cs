using api_intiSoft.Models.CentroEstudios;
using intiSoft;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Controllers.CentroEstudios
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColegiosController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public ColegiosController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/Colegios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Colegio>>> GetColegio()
        {
            return await _context.Colegio.ToListAsync();
        }

        // GET: api/Colegios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Colegio>> GetColegio(int id)
        {
            var colegio = await _context.Colegio.FindAsync(id);

            if (colegio == null)
            {
                return NotFound();
            }

            return colegio;
        }

        // PUT: api/Colegios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutColegio(int id, Colegio colegio)
        {
            if (id != colegio.Id)
            {
                return BadRequest();
            }

            _context.Entry(colegio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ColegioExists(id))
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

        // POST: api/Colegios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Colegio>> PostColegio(Colegio colegio)
        {
            _context.Colegio.Add(colegio);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetColegio", new { id = colegio.Id }, colegio);
        }

        //patch
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchColegio(int id, Colegio colegio)
        {
            if (id != colegio.Id)
            {
                return BadRequest();
            }
            var existingColegio = await _context.Colegio.FindAsync(id);
            if (existingColegio == null)
            {
                return NotFound();
            }
            // Update only the fields that are not null in the incoming object
            if (colegio.Nombre != null)
                existingColegio.Nombre = colegio.Nombre;
            if (colegio.UbigeoCode != null)
                existingColegio.UbigeoCode = colegio.UbigeoCode;
            if (colegio.EsPrivado.HasValue)
                existingColegio.EsPrivado = colegio.EsPrivado;
            if (colegio.Activo != existingColegio.Activo) // Assuming 'Activo' is a bool and should always be updated
                existingColegio.Activo = colegio.Activo;
            if (colegio.FechaRegistro.HasValue)
                existingColegio.FechaRegistro = colegio.FechaRegistro;
            if (colegio.FechaActualizacion.HasValue)
                existingColegio.FechaActualizacion = colegio.FechaActualizacion;
            if (colegio.UsuaraioRegistroId.HasValue)
                existingColegio.UsuaraioRegistroId = colegio.UsuaraioRegistroId;
            if (colegio.UsuaraioActualizacionId.HasValue)
                existingColegio.UsuaraioActualizacionId = colegio.UsuaraioActualizacionId;
            _context.Entry(existingColegio).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ColegioExists(id))
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

        // DELETE: api/Colegios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteColegio(int id)
        {
            var colegio = await _context.Colegio.FindAsync(id);
            if (colegio == null)
            {
                return NotFound();
            }

            _context.Colegio.Remove(colegio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ColegioExists(int id)
        {
            return _context.Colegio.Any(e => e.Id == id);
        }
    }
}
