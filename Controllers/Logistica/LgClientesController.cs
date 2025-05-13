using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_intiSoft.Models.Logistica.Producto;
using intiSoft;

namespace api_intiSoft.Controllers.Logistica
{
    [Route("api/[controller]")]
    [ApiController]
    public class LgClientesController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public LgClientesController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/LgClientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LgCliente>>> GetLgCliente()
        {
            return await _context.LgCliente.ToListAsync();
        }

        // GET: api/LgClientes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LgCliente>> GetLgCliente(int id)
        {
            var lgCliente = await _context.LgCliente.FindAsync(id);

            if (lgCliente == null)
            {
                return NotFound();
            }

            return lgCliente;
        }
        //buscar si existe  DocumentoIdentidadId y NumeroDocumento
        [HttpGet("existsdocumento")]
        public async Task<ActionResult<bool>> VerificarExistenciaCliente(
            [FromQuery] int clienteId,
            [FromQuery] string documentoIdentidadId, // Changed from int to string
            [FromQuery] string numeroDocumento)
        {
            if (string.IsNullOrWhiteSpace(numeroDocumento))
                return Ok(false); // No se puede verificar duplicado sin número válido
            if (string.IsNullOrWhiteSpace(documentoIdentidadId))
                return Ok(false); // No se puede verificar duplicado sin documento válido            
            if (clienteId <= 0)
                return Ok(false); // No se puede verificar duplicado sin clienteId válido


            bool existe = await _context.LgCliente.AnyAsync(c =>
                c.ClienteId != clienteId &&
                c.DocumentoIdentidadId == documentoIdentidadId &&
                c.NumeroDocumento == numeroDocumento);

            return Ok(existe); // true o false, directamente
        }


        //get by documento o apellidos y nombre o  razon social
        //[HttpGet("search")]
        //public async Task<ActionResult<IEnumerable<LgCliente>>> SearchFullText([FromQuery] string datosBuscar)
        [HttpGet("search/{datosBuscar}")]
        public async Task<ActionResult<IEnumerable<LgCliente>>> SearchFullText(string datosBuscar)
        {
            if (string.IsNullOrWhiteSpace(datosBuscar))
                return BadRequest("Debe ingresar texto para buscar.");

            var resultados = await _context.LgCliente
                .FromSqlRaw("""
                SELECT * FROM Lg_Cliente 
                WHERE SearchVector @@ plainto_tsquery('spanish', {0}) LIMIT 30
                """, datosBuscar).ToListAsync();

            if (!resultados.Any())
                    return NotFound();

                return Ok(resultados);
        }

        // PUT: api/LgClientes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLgCliente(int id, LgCliente lgCliente)
        {
            if (id != lgCliente.ClienteId)
            {
                return BadRequest();
            }
            // Asegura que las fechas sean UTC
            //lgCliente.FechaRegistro = DateTime.SpecifyKind(lgCliente.FechaRegistro, DateTimeKind.Utc);
            lgCliente.FechaActualizacion = DateTime.UtcNow;
            lgCliente.FechaNacimiento = ConvertToUtc(lgCliente.FechaNacimiento);



            _context.Entry(lgCliente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LgClienteExists(id))
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

        // POST: api/LgClientes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LgCliente>> PostLgCliente(LgCliente lgCliente)
        {
            // Asegura que las fechas sean UTC
            lgCliente.FechaRegistro = DateTime.UtcNow;
            //lgCliente.Activo = true; // Asignar valor por defecto
            lgCliente.FechaActualizacion = null;
            lgCliente.FechaNacimiento = ConvertToUtc(lgCliente.FechaNacimiento);


            _context.LgCliente.Add(lgCliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLgCliente", new { id = lgCliente.ClienteId }, lgCliente);
        }

        // DELETE: api/LgClientes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLgCliente(int id)
        {
            var lgCliente = await _context.LgCliente.FindAsync(id);
            if (lgCliente == null)
            {
                return NotFound();
            }

            _context.LgCliente.Remove(lgCliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LgClienteExists(int id)
        {
            return _context.LgCliente.Any(e => e.ClienteId == id);
        }
        private static DateTime? ConvertToUtc(DateTime? dt)
        {
            return dt.HasValue ? dt.Value.ToUniversalTime() : null;
        }
    }
}
