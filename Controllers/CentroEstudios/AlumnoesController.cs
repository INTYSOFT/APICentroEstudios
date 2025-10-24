using api_intiSoft.Dto.CentroEstudios;
using api_intiSoft.Models.CentroEstudios;
using intiSoft;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_intiSoft.Controllers.CentroEstudios
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlumnoesController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public AlumnoesController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/Alumnoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Alumno>>> GetAlumno()
        {
            return await _context.Alumno.ToListAsync();
        }

        // GET: api/Alumnoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Alumno>> GetAlumno(int id)
        {
            var alumno = await _context.Alumno.FindAsync(id);

            if (alumno == null)
            {
                return NotFound();
            }

            return alumno;
        }
        //Get Alumnos por dni
        [HttpGet("dni/{dni}")]
        public async Task<ActionResult<IEnumerable<Alumno>>> GetAlumnoByDni(string dni)
        {
            var alumnos = await _context.Alumno.Where(a => a.Dni == dni).ToListAsync();
            if (alumnos == null || alumnos.Count == 0)
            {
                // return vacio
                return new List<Alumno>();
            }
            return alumnos;

        }


        // PUT: api/Alumnoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAlumno(int id, Alumno alumno)
        {
            if (id != alumno.Id)
            {
                return BadRequest();
            }

            _context.Entry(alumno).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlumnoExists(id))
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

        // POST: api/Alumnoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Alumno>> PostAlumno([FromBody] AlumnoCreateDto dto)
        {
            // 1) Normaliza apellidos
            var apellidos = string.IsNullOrWhiteSpace(dto.Apellidos) ? null : dto.Apellidos;


            // 2) Parse seguro a DateOnly (acepta "yyyy-MM-dd" o ISO con hora)
            DateOnly? fechaNac = null;
            if (!string.IsNullOrWhiteSpace(dto.FechaNacimiento))
            {
                // intenta "yyyy-MM-dd"
                if (DateOnly.TryParse(dto.FechaNacimiento, out var d1))
                    fechaNac = d1;
                else if (DateTimeOffset.TryParse(dto.FechaNacimiento, out var dtoff))
                    fechaNac = DateOnly.FromDateTime(dtoff.DateTime);
                else
                    return BadRequest("FechaNacimiento inválida");
            }

            var alumno = new Alumno
            {
                Dni = dto.Dni,
                Apellidos = string.IsNullOrWhiteSpace(apellidos) ? null : apellidos,
                Nombres = dto.Nombres,
                Celular = dto.Celular,
                Correo = dto.Correo,
                ColegioId = dto.ColegioId,
                UbigeoCode = dto.UbigeoCode,
                Observacion = dto.Observacion,
                FotoUrl = dto.FotoUrl,
                Direccion = dto.Direccion,
                // Direccion no existe en la entidad: ignóralo o agrégalo al modelo si corresponde
                FechaNacimiento = fechaNac,
                Activo = dto.Activo,
                FechaRegistro = DateTime.UtcNow
            };

            _context.Alumno.Add(alumno);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAlumno), new { id = alumno.Id }, alumno);
        }


        //Patch
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchAlumno(int id, [FromBody] AlumnoCreateDto dto)
        {
            var existing = await _context.Alumno.FindAsync(id);
            if (existing == null) return NotFound();

            // Mapear solo lo que viene
            if (dto.Dni != null) existing.Dni = dto.Dni;
            if (dto.Apellidos != null) existing.Apellidos = dto.Apellidos;
            if (dto.Nombres != null) existing.Nombres = dto.Nombres;
            if (dto.Celular != null) existing.Celular = dto.Celular;
            if (dto.Correo != null) existing.Correo = dto.Correo;
            if (dto.ColegioId != null) existing.ColegioId = dto.ColegioId;
            if (dto.UbigeoCode != null) existing.UbigeoCode = dto.UbigeoCode;
            if (dto.Observacion != null) existing.Observacion = dto.Observacion;
            if (dto.FotoUrl != null) existing.FotoUrl = dto.FotoUrl;
            if (dto.Direccion != null) existing.Direccion = dto.Direccion;



            if (!string.IsNullOrWhiteSpace(dto.FechaNacimiento))
            {
                if (DateOnly.TryParse(dto.FechaNacimiento, out var d1))
                    existing.FechaNacimiento = d1;
                else if (DateTimeOffset.TryParse(dto.FechaNacimiento, out var dtoff))
                    existing.FechaNacimiento = DateOnly.FromDateTime(dtoff.DateTime);
                else
                    return BadRequest("FechaNacimiento inválida");
            }

            if (dto.Activo.ToString() != null) existing.Activo = dto.Activo;

            existing.FechaActualizacion = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }



        // DELETE: api/Alumnoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlumno(int id)
        {
            var alumno = await _context.Alumno.FindAsync(id);
            if (alumno == null)
            {
                return NotFound();
            }

            _context.Alumno.Remove(alumno);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AlumnoExists(int id)
        {
            return _context.Alumno.Any(e => e.Id == id);
        }
    }
}
