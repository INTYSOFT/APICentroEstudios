using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_intiSoft.Models.Clinica;
using intiSoft;
using Npgsql;

namespace api_intiSoft.Controllers.Clinica
{
    [Route("api/[controller]")]
    [ApiController]
    public class GmHorarioAperturasController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public GmHorarioAperturasController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/GmHorarioAperturas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GmHorarioApertura>>> GetGmHorarioApertura()
        {
            return await _context.GmHorarioApertura.ToListAsync();
        }

        // GET: api/GmHorarioAperturas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GmHorarioApertura>> GetGmHorarioApertura(int id)
        {
            var gmHorarioApertura = await _context.GmHorarioApertura.FindAsync(id);

            if (gmHorarioApertura == null)
            {
                return NotFound();
            }

            return gmHorarioApertura;
        }
        // Updated method to handle nullable reference types properly
        [HttpGet("gethorarios")]
        public async Task<ActionResult<IEnumerable<GmHorarioApertura>>> GetHorarios(
            [FromQuery] int sucursalId,
            [FromQuery] int anio,
            [FromQuery] int mes)
        {
            var gmHorarioApertura = await _context.GmHorarioApertura
                .Where(h => h.SucursalId == sucursalId
                            && h.Fecha.HasValue
                            && h.Fecha.Value.Year == anio
                            && h.Fecha.Value.Month == mes)
                .ToListAsync();

            if (gmHorarioApertura == null || !gmHorarioApertura.Any())
            {
                return Ok(gmHorarioApertura);
            }

            return Ok(gmHorarioApertura);
        }

        // PUT: api/GmHorarioAperturas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGmHorarioApertura(int id, GmHorarioApertura gmHorarioApertura)
        {
            if (id != gmHorarioApertura.HorarioAperturaId)
            {
                return BadRequest(new { message = "El ID no coincide con el objeto enviado." });
            }

            // Asegurar tipos de fecha sin zona horaria
            gmHorarioApertura.FechaRegistro = gmHorarioApertura.FechaRegistro.HasValue
                ? DateTime.SpecifyKind(gmHorarioApertura.FechaRegistro.Value, DateTimeKind.Unspecified)
                : null;

            gmHorarioApertura.FechaActualizacion = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
            gmHorarioApertura.Fecha = gmHorarioApertura.Fecha;

            _context.Entry(gmHorarioApertura).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GmHorarioAperturaExists(id))
                {
                    return NotFound(new { message = "Horario no encontrado." });
                }
                else
                {
                    throw;
                }
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
                {
                    return BadRequest(new
                    {
                        message = "Ya existe un horario con la misma fecha, hora de inicio, hora de fin y sucursal.",
                        error = "Llave duplicada"
                    });
                }

                return StatusCode(500, new
                {
                    message = "Error al actualizar el horario en la base de datos.",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error interno del servidor.",
                    detail = ex.Message
                });
            }

            return NoContent();
        }




        // POST: api/GmHorarioAperturas
        [HttpPost]
        public async Task<ActionResult<GmHorarioApertura>> PostGmHorarioApertura(GmHorarioApertura gmHorarioApertura)
        {
            try
            {
                _context.GmHorarioApertura.Add(gmHorarioApertura);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetGmHorarioApertura", new { id = gmHorarioApertura.HorarioAperturaId }, gmHorarioApertura);
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
                {
                    return BadRequest(new
                    {
                        message = "Ya existe un horario con la misma fecha, hora de inicio, hora de fin y sucursal.",
                        error = "Llave duplicada"
                    });
                }

                // Otro tipo de error de base de datos
                return StatusCode(500, new
                {
                    message = "Error al guardar el horario en la base de datos.",
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                // Error inesperado
                return StatusCode(500, new
                {
                    message = "Error interno del servidor.",
                    detail = ex.Message
                });
            }
        }


        // DELETE: api/GmHorarioAperturas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGmHorarioApertura(int id)
        {
            var gmHorarioApertura = await _context.GmHorarioApertura.FindAsync(id);
            if (gmHorarioApertura == null)
            {
                return NotFound();
            }

            _context.GmHorarioApertura.Remove(gmHorarioApertura);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GmHorarioAperturaExists(int id)
        {
            return _context.GmHorarioApertura.Any(e => e.HorarioAperturaId == id);
        }

     
    }
}
