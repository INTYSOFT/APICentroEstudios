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
    public class LgDetalleListaFichaTecnicasController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public LgDetalleListaFichaTecnicasController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/LgDetalleListaFichaTecnicas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LgDetalleListaFichaTecnica>>> GetLgDetalleListaFichaTecnica()
        {
            return await _context.LgDetalleListaFichaTecnica.ToListAsync();
        }

        // GET: api/LgDetalleListaFichaTecnicas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LgDetalleListaFichaTecnica>> GetLgDetalleListaFichaTecnica(int id)
        {
            var lgDetalleListaFichaTecnica = await _context.LgDetalleListaFichaTecnica.FindAsync(id);

            if (lgDetalleListaFichaTecnica == null)
            {
                return NotFound();
            }

            return lgDetalleListaFichaTecnica;
        }

        //get LgDetalleListaFichaTecnica por listaFichaTecnicaId
        [HttpGet("listaFichaTecnica/{id}")]
        public async Task<ActionResult<IEnumerable<LgDetalleListaFichaTecnica>>> GetLgDetalleListaFichaTecnicaByListaFichaTecnicaId(int id)
        {
            return await _context.LgDetalleListaFichaTecnica.Where(x => x.ListaFichaTecnicaId == id).ToListAsync();
        }





        // PUT: api/LgDetalleListaFichaTecnicas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLgDetalleListaFichaTecnica(int id, LgDetalleListaFichaTecnica lgDetalleListaFichaTecnica)
        {
            if (id != lgDetalleListaFichaTecnica.DetalleListaFichaTecnicaId)
            {
                return BadRequest();
            }

            _context.Entry(lgDetalleListaFichaTecnica).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LgDetalleListaFichaTecnicaExists(id))
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

        // POST: api/LgDetalleListaFichaTecnicas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LgDetalleListaFichaTecnica>> PostLgDetalleListaFichaTecnica(LgDetalleListaFichaTecnica lgDetalleListaFichaTecnica)
        {
            _context.LgDetalleListaFichaTecnica.Add(lgDetalleListaFichaTecnica);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLgDetalleListaFichaTecnica", new { id = lgDetalleListaFichaTecnica.DetalleListaFichaTecnicaId }, lgDetalleListaFichaTecnica);
        }
        
        

        // DELETE: api/LgDetalleListaFichaTecnicas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLgDetalleListaFichaTecnica(int id)
        {
            var lgDetalleListaFichaTecnica = await _context.LgDetalleListaFichaTecnica.FindAsync(id);
            if (lgDetalleListaFichaTecnica == null)
            {
                return NotFound();
            }

            _context.LgDetalleListaFichaTecnica.Remove(lgDetalleListaFichaTecnica);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LgDetalleListaFichaTecnicaExists(int id)
        {
            return _context.LgDetalleListaFichaTecnica.Any(e => e.DetalleListaFichaTecnicaId == id);
        }
    }
}
