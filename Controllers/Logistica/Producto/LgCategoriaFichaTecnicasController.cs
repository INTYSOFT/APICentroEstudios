using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_intiSoft.Models.Logistica.Producto;
using intiSoft;
using api_intiSoft.Service;
using api_intiSoft.Dto.Logistica.Producto;

namespace api_intiSoft.Controllers.Logistica.Producto
{
    [Route("api/[controller]")]
    [ApiController]
    public class LgCategoriaFichaTecnicasController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;
        private readonly IFichaTecnicaService _fichaTecnicaService;


        public LgCategoriaFichaTecnicasController(ConecDinamicaContext context, IFichaTecnicaService fichaTecnicaService)
        {
            _context = context;
            _fichaTecnicaService = fichaTecnicaService;
        }
        

        //get por categoria_id
        [HttpGet("GetByCategoriaId/{categoria_id}")]
        public async Task<ActionResult<List<CategoriaFichaTecnicaDetalleDto>>> GetByCategoriaId( int categoria_id)
        {
            var fichasTecnicas = await _fichaTecnicaService.GetCategoriaFichaByCategoriaId(categoria_id);
            return Ok(fichasTecnicas);
        }

       
        // GET: api/LgCategoriaFichaTecnicas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LgCategoriaFichaTecnica>>> GetLgCategoriaFichaTecnica()
        {
            return await _context.LgCategoriaFichaTecnica.ToListAsync();
        }

        // GET: api/LgCategoriaFichaTecnicas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LgCategoriaFichaTecnica>> GetLgCategoriaFichaTecnica(int id)
        {
            var lgCategoriaFichaTecnica = await _context.LgCategoriaFichaTecnica.FindAsync(id);

            if (lgCategoriaFichaTecnica == null)
            {
                return NotFound();
            }

            return lgCategoriaFichaTecnica;
        }

        // PUT: api/LgCategoriaFichaTecnicas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLgCategoriaFichaTecnica(int id, LgCategoriaFichaTecnica lgCategoriaFichaTecnica)
        {
            if (id != lgCategoriaFichaTecnica.CategoriaFichaTecnicaId)
            {
                return BadRequest();
            }

            _context.Entry(lgCategoriaFichaTecnica).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LgCategoriaFichaTecnicaExists(id))
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

        // POST: api/LgCategoriaFichaTecnicas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LgCategoriaFichaTecnica>> PostLgCategoriaFichaTecnica(LgCategoriaFichaTecnica lgCategoriaFichaTecnica)
        {
            _context.LgCategoriaFichaTecnica.Add(lgCategoriaFichaTecnica);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLgCategoriaFichaTecnica", new { id = lgCategoriaFichaTecnica.CategoriaFichaTecnicaId }, lgCategoriaFichaTecnica);
        }

        // DELETE: api/LgCategoriaFichaTecnicas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLgCategoriaFichaTecnica(int id)
        {
            var lgCategoriaFichaTecnica = await _context.LgCategoriaFichaTecnica.FindAsync(id);
            if (lgCategoriaFichaTecnica == null)
            {
                return NotFound();
            }

            _context.LgCategoriaFichaTecnica.Remove(lgCategoriaFichaTecnica);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LgCategoriaFichaTecnicaExists(int id)
        {
            return _context.LgCategoriaFichaTecnica.Any(e => e.CategoriaFichaTecnicaId == id);
        }
    }
}
