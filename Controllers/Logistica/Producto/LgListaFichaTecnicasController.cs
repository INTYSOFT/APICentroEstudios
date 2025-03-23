using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_intiSoft.Models.Logistica.Producto;
using intiSoft;

namespace api_intiSoft.Controllers.Logistica.Producto
{
    [Route("api/[controller]")]
    [ApiController]
    public class LgListaFichaTecnicasController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public LgListaFichaTecnicasController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/LgListaFichaTecnicas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LgListaFichaTecnica>>> GetLgListaFichaTecnica()
        {
            return await _context.LgListaFichaTecnica.ToListAsync();
        }

        // GET: api/LgListaFichaTecnicas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LgListaFichaTecnica>> GetLgListaFichaTecnica(int id)
        {
            var lgListaFichaTecnica = await _context.LgListaFichaTecnica.FindAsync(id);

            if (lgListaFichaTecnica == null)
            {
                return NotFound();
            }

            return lgListaFichaTecnica;
        }

        // PUT: api/LgListaFichaTecnicas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLgListaFichaTecnica(int id, LgListaFichaTecnica lgListaFichaTecnica)
        {
            if (id != lgListaFichaTecnica.ListaFichaTecnicaId)
            {
                return BadRequest();
            }

            _context.Entry(lgListaFichaTecnica).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LgListaFichaTecnicaExists(id))
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

        // POST: api/LgListaFichaTecnicas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LgListaFichaTecnica>> PostLgListaFichaTecnica(LgListaFichaTecnica lgListaFichaTecnica)
        {
            _context.LgListaFichaTecnica.Add(lgListaFichaTecnica);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLgListaFichaTecnica", new { id = lgListaFichaTecnica.ListaFichaTecnicaId }, lgListaFichaTecnica);
        }

        // DELETE: api/LgListaFichaTecnicas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLgListaFichaTecnica(int id)
        {
            var lgListaFichaTecnica = await _context.LgListaFichaTecnica.FindAsync(id);
            if (lgListaFichaTecnica == null)
            {
                return NotFound();
            }

            _context.LgListaFichaTecnica.Remove(lgListaFichaTecnica);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LgListaFichaTecnicaExists(int id)
        {
            return _context.LgListaFichaTecnica.Any(e => e.ListaFichaTecnicaId == id);
        }
    }
}
