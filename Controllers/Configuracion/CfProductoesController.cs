using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_intiSoft.Models.Configuracion;
using intiSoft;

namespace api_intiSoft.Controllers.Configuracion
{
    [Route("api/[controller]")]
    [ApiController]
    public class CfProductoesController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public CfProductoesController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/CfProductoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CfProducto>>> GetCfProducto()
        {
            return await _context.CfProducto.ToListAsync();
        }

        // GET: api/CfProductoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CfProducto>> GetCfProducto(int id)
        {
            var cfProducto = await _context.CfProducto.FindAsync(id);

            if (cfProducto == null)
            {
                return NotFound();
            }

            return cfProducto;
        }

        // PUT: api/CfProductoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCfProducto(int id, CfProducto cfProducto)
        {
            if (id != cfProducto.ProductoConfigId)
            {
                return BadRequest();
            }

            _context.Entry(cfProducto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CfProductoExists(id))
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

        // POST: api/CfProductoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CfProducto>> PostCfProducto(CfProducto cfProducto)
        {
            _context.CfProducto.Add(cfProducto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCfProducto", new { id = cfProducto.ProductoConfigId }, cfProducto);
        }

        // DELETE: api/CfProductoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCfProducto(int id)
        {
            var cfProducto = await _context.CfProducto.FindAsync(id);
            if (cfProducto == null)
            {
                return NotFound();
            }

            _context.CfProducto.Remove(cfProducto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CfProductoExists(int id)
        {
            return _context.CfProducto.Any(e => e.ProductoConfigId == id);
        }
    }
}
