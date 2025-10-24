using api_intiSoft.Models.CentroEstudios;
using intiSoft;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Controllers.CentroEstudios
{
    [Route("api/[controller]")]
    [ApiController]
    public class SedesController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public SedesController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/Sedes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sede>>> GetSede()
        {
            return await _context.Sede.ToListAsync();
        }

        // GET: api/Sedes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Sede>> GetSede(int id)
        {
            var sede = await _context.Sede.FindAsync(id);

            if (sede == null)
            {
                return NotFound();
            }

            return sede;
        }

        // PUT: api/Sedes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSede(int id, Sede sede)
        {
            if (id != sede.Id)
            {
                return BadRequest();
            }

            _context.Entry(sede).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SedeExists(id))
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

        // POST: api/Sedes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Sede>> PostSede(Sede sede)
        {
            _context.Sede.Add(sede);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSede", new { id = sede.Id }, sede);
        }

        //PATCH
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchSede(int id, Sede sede)
        {
            if (id != sede.Id)
            {
                return BadRequest();
            }
            var existingSede = await _context.Sede.FindAsync(id);
            if (existingSede == null)
            {
                return NotFound();
            }
            // Update only the fields that are not null in the incoming sede object
            if (sede.Nombre != null)
                existingSede.Nombre = sede.Nombre;
            if (sede.UbigeoCode != null)
                existingSede.UbigeoCode = sede.UbigeoCode;
            if (sede.Direccion != null)
                existingSede.Direccion = sede.Direccion;
            existingSede.Activo = sede.Activo; // Assuming boolean fields should always be updated
            if (sede.FechaRegistro != null)
                existingSede.FechaRegistro = sede.FechaRegistro;
            if (sede.FechaActualizacion != null)
                existingSede.FechaActualizacion = sede.FechaActualizacion;
            if (sede.UsuaraioRegistroId != null)
                existingSede.UsuaraioRegistroId = sede.UsuaraioRegistroId;
            if (sede.UsuaraioActualizacionId != null)
                existingSede.UsuaraioActualizacionId = sede.UsuaraioActualizacionId;
            _context.Entry(existingSede).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SedeExists(id))
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

        // DELETE: api/Sedes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSede(int id)
        {
            var sede = await _context.Sede.FindAsync(id);
            if (sede == null)
            {
                return NotFound();
            }

            _context.Sede.Remove(sede);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SedeExists(int id)
        {
            return _context.Sede.Any(e => e.Id == id);
        }
    }
}
