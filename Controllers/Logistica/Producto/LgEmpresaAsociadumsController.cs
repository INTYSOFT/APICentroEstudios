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
    public class LgEmpresaAsociadumsController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public LgEmpresaAsociadumsController(ConecDinamicaContext context)
        {
            _context = context;
        }

        // GET: api/LgEmpresaAsociadums
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LgEmpresaAsociadum>>> GetLgEmpresaAsociadum()
        {
            return await _context.LgEmpresaAsociadum.ToListAsync();
        }

        // GET: api/LgEmpresaAsociadums/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LgEmpresaAsociadum>> GetLgEmpresaAsociadum(int id)
        {
            var lgEmpresaAsociadum = await _context.LgEmpresaAsociadum.FindAsync(id);

            if (lgEmpresaAsociadum == null)
            {
                return NotFound();
            }

            return lgEmpresaAsociadum;
        }

        // PUT: api/LgEmpresaAsociadums/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLgEmpresaAsociadum(int id, LgEmpresaAsociadum lgEmpresaAsociadum)
        {
            if (id != lgEmpresaAsociadum.EmpresaAsociadaId)
            {
                return BadRequest();
            }

            _context.Entry(lgEmpresaAsociadum).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LgEmpresaAsociadumExists(id))
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

        // POST: api/LgEmpresaAsociadums
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LgEmpresaAsociadum>> PostLgEmpresaAsociadum(LgEmpresaAsociadum lgEmpresaAsociadum)
        {
            _context.LgEmpresaAsociadum.Add(lgEmpresaAsociadum);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLgEmpresaAsociadum", new { id = lgEmpresaAsociadum.EmpresaAsociadaId }, lgEmpresaAsociadum);
        }

        // DELETE: api/LgEmpresaAsociadums/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLgEmpresaAsociadum(int id)
        {
            var lgEmpresaAsociadum = await _context.LgEmpresaAsociadum.FindAsync(id);
            if (lgEmpresaAsociadum == null)
            {
                return NotFound();
            }

            _context.LgEmpresaAsociadum.Remove(lgEmpresaAsociadum);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LgEmpresaAsociadumExists(int id)
        {
            return _context.LgEmpresaAsociadum.Any(e => e.EmpresaAsociadaId == id);
        }
    }
}
