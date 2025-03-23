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
    public class LgEmpresaAsociadaProductoesController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public LgEmpresaAsociadaProductoesController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/LgEmpresaAsociadaProductoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LgEmpresaAsociadaProducto>>> GetLgEmpresaAsociadaProducto()
        {
            return await _context.LgEmpresaAsociadaProducto.ToListAsync();
        }

        // GET: api/LgEmpresaAsociadaProductoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LgEmpresaAsociadaProducto>> GetLgEmpresaAsociadaProducto(int id)
        {
            var lgEmpresaAsociadaProducto = await _context.LgEmpresaAsociadaProducto.FindAsync(id);

            if (lgEmpresaAsociadaProducto == null)
            {
                return NotFound();
            }

            return lgEmpresaAsociadaProducto;
        }

        // PUT: api/LgEmpresaAsociadaProductoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLgEmpresaAsociadaProducto(int id, LgEmpresaAsociadaProducto lgEmpresaAsociadaProducto)
        {
            if (id != lgEmpresaAsociadaProducto.EmpresaAsociadaProductoId)
            {
                return BadRequest();
            }

            _context.Entry(lgEmpresaAsociadaProducto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LgEmpresaAsociadaProductoExists(id))
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

        // POST: api/LgEmpresaAsociadaProductoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LgEmpresaAsociadaProducto>> PostLgEmpresaAsociadaProducto(LgEmpresaAsociadaProducto lgEmpresaAsociadaProducto)
        {
            _context.LgEmpresaAsociadaProducto.Add(lgEmpresaAsociadaProducto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLgEmpresaAsociadaProducto", new { id = lgEmpresaAsociadaProducto.EmpresaAsociadaProductoId }, lgEmpresaAsociadaProducto);
        }

        // DELETE: api/LgEmpresaAsociadaProductoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLgEmpresaAsociadaProducto(int id)
        {
            var lgEmpresaAsociadaProducto = await _context.LgEmpresaAsociadaProducto.FindAsync(id);
            if (lgEmpresaAsociadaProducto == null)
            {
                return NotFound();
            }

            _context.LgEmpresaAsociadaProducto.Remove(lgEmpresaAsociadaProducto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LgEmpresaAsociadaProductoExists(int id)
        {
            return _context.LgEmpresaAsociadaProducto.Any(e => e.EmpresaAsociadaProductoId == id);
        }
    }
}
