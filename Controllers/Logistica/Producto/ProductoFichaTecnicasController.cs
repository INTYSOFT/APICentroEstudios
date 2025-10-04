using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_intiSoft.Models.Logistica.Producto;
using intiSoft;
using AutoMapper;
using api_intiSoft.Dto.Logistica.Producto;

namespace api_intiSoft.Controllers.Logistica.Producto
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoFichaTecnicasController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;
        private readonly IMapper _mapper;

        public ProductoFichaTecnicasController(ConecDinamicaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/LgProductoFichaTecnicas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoFichaTecnica>>> GetLgProductoFichaTecnica()
        {
            return await _context.LgProductoFichaTecnica.ToListAsync();
        }

        // GET: api/LgProductoFichaTecnicas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoFichaTecnica>> GetLgProductoFichaTecnica(int id)
        {
            var lgProductoFichaTecnica = await _context.LgProductoFichaTecnica.FindAsync(id);

            if (lgProductoFichaTecnica == null)
            {
                return NotFound();
            }

            return lgProductoFichaTecnica;
        }

        //get by productoId
        [HttpGet("byproductoid/{productoId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetLgProductoFichaTecnicaByProductoId(int productoId)
        {
            var result = await _context.LgProductoFichaTecnica
                .Where(p => p.ProductoId == productoId)
                .Select(p => new
                {
                    p.ProductoFichaTecnicaId,
                    p.ProductoId,
                    p.FichaTecnicaId,
                    p.Descripcion,
                    p.Activo,
                    p.CategoriaFichaTecnicaDetalleId,
                    p.CategoriaFichaTecnicaId,
                    p.Nombre,
                    p.ListaFichaTecnicaId,                 
                    FichaTecnica = p.FichaTecnica == null ? null : new
                    {
                        p.FichaTecnica.FichaTecnicaId,
                        p.FichaTecnica.Nombre,
                        p.FichaTecnica.Descripcion,
                        p.FichaTecnica.Activo
                    },
                    LgProductoFichaTecnicaDetalles = p.ProductoFichaTecnicaDetalles.Select(d => new
                    {
                        d.ProductoFichaTecnicaDetalleId,
                        d.ProductoFichaTecnicaId,
                        d.DetalleListaFichaTecnicaId,
                        d.Dato,
                        d.Descripcion,
                        d.Activo
                    }).ToList()
                })
                .ToListAsync();

            return Ok(result);
        }        




        // PUT: api/LgProductoFichaTecnicas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLgProductoFichaTecnica(int id, ProductoFichaTecnica lgProductoFichaTecnica)
        {
            if (id != lgProductoFichaTecnica.ProductoFichaTecnicaId)
            {
                return BadRequest();
            }

            _context.Entry(lgProductoFichaTecnica).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LgProductoFichaTecnicaExists(id))
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


        // POST: api/LgProductoFichaTecnicas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductoFichaTecnica>> PostLgProductoFichaTecnica(ProductoFichaTecnica lgProductoFichaTecnica)
        {
            _context.LgProductoFichaTecnica.Add(lgProductoFichaTecnica);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLgProductoFichaTecnica", new { id = lgProductoFichaTecnica.ProductoFichaTecnicaId }, lgProductoFichaTecnica);
        }     

        // DELETE: api/LgProductoFichaTecnicas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLgProductoFichaTecnica(int id)
        {
            var lgProductoFichaTecnica = await _context.LgProductoFichaTecnica.FindAsync(id);
            if (lgProductoFichaTecnica == null)
            {
                return NotFound();
            }

            _context.LgProductoFichaTecnica.Remove(lgProductoFichaTecnica);
            await _context.SaveChangesAsync();

            return NoContent();
        }
       


        [HttpPost("postmultiplewithdetalle")]
        public async Task<IActionResult> PostLgProductoFichaTecnicaMultipleWithDetalleSinIds([FromBody] List<LgProductoFichaTecnicaDto> dtos)
        {
            if (dtos == null || !dtos.Any())
                return BadRequest(new { 
                    codigo = StatusCodes.Status400BadRequest,
                    mensaje = "Debe proporcionar al menos una ficha técnica." });

            try
            {
                var entidades = _mapper.Map<List<ProductoFichaTecnica>>(dtos);

                await using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    await _context.LgProductoFichaTecnica.AddRangeAsync(entidades);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    //return ok con mensaje de "fichas técnicas guardadas correctamente"
                    return Ok(new { 
                        codigo = StatusCodes.Status200OK,
                        mensaje = "Fichas técnicas guardadas correctamente." ,
                        detalle = "Se han guardado " + entidades.Count + " fichas técnicas.",
                    }
                    );
                }
                catch (DbUpdateException dbEx)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(StatusCodes.Status500InternalServerError, new
                    {
                        codigo = StatusCodes.Status500InternalServerError,
                        mensaje = "Error al guardar en la base de datos.",
                        detalle = dbEx.InnerException?.Message ?? dbEx.Message
                    });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(StatusCodes.Status500InternalServerError, new
                    {
                        codigo = StatusCodes.Status500InternalServerError,
                        mensaje = "Error interno al procesar la solicitud.",
                        detalle = ex.InnerException?.Message ?? ex.Message
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    codigo = StatusCodes.Status500InternalServerError,
                    mensaje = "Ocurrió un error inesperado.",
                    detalle = ex.Message
                });
            }
        }

        //put multiple LgProductoFichaTecnicaDto.
        [HttpPut("putmultiple")]
        public async Task<IActionResult> PutLgProductoFichaTecnicaMultiple([FromBody] List<LgProductoFichaTecnicaDto> dtos)
        {
            if (dtos == null || dtos.Count == 0)
                return BadRequest(new { codigo = 400, mensaje = "Debe proporcionar al menos una ficha técnica." });

            // ── Validación: todos deben traer Id (>0)
            var ids = dtos.Select(d => d.ProductoFichaTecnicaId).Distinct().ToList();
            if (ids.Any(id => id <= 0))
                return BadRequest(new { codigo = 400, mensaje = "Todos los elementos deben incluir 'productoFichaTecnicaId' válido (>0)." });

            await using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1) Cargar entidades actuales (TRACKED) con sus detalles
                var actuales = await _context.LgProductoFichaTecnica
                    .Include(p => p.ProductoFichaTecnicaDetalles)
                    .Where(p => ids.Contains(p.ProductoFichaTecnicaId))
                    .ToListAsync();

                // 2) Validar faltantes
                var faltantes = ids.Except(actuales.Select(a => a.ProductoFichaTecnicaId)).ToList();
                if (faltantes.Count > 0)
                    return NotFound(new { codigo = 404, mensaje = "Id(s) no encontrados.", ids = faltantes });

                // 3) Aplicar cambios (padre + reconciliar hijos)
                foreach (var dto in dtos)
                {
                    var entity = actuales.First(a => a.ProductoFichaTecnicaId == dto.ProductoFichaTecnicaId);

                    // Mapear ESCALARES del padre (navegaciones ignoradas por el Profile)
                    _mapper.Map(dto, entity);

                    // Reconciliar detalles (upsert)
                    ReconciliarDetalles(entity, dto.LgProductoFichaTecnicaDetalles);
                }

                await _context.SaveChangesAsync();
                await tx.CommitAsync();

                return Ok(new
                {
                    codigo = 200,
                    mensaje = "Fichas técnicas actualizadas correctamente.",
                    detalle = $"Se han actualizado {dtos.Count} fichas técnicas."
                });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                await tx.RollbackAsync();
                return StatusCode(409, new
                {
                    codigo = 409,
                    mensaje = "Conflicto de concurrencia al actualizar.",
                    detalle = ex.InnerException?.Message ?? ex.Message
                });
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                return StatusCode(500, new
                {
                    codigo = 500,
                    mensaje = "Error interno al procesar la solicitud.",
                    detalle = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        // === ayuda: reconciliación de detalles ===
        private static void ReconciliarDetalles(
            ProductoFichaTecnica entity,
            List<LgProductoFichaTecnicaDetalleDto> dtoDetalles)
        {
            dtoDetalles ??= new();

            // Índice por Id para buscar rápido
            var actualesPorId = entity.ProductoFichaTecnicaDetalles
                .ToDictionary(d => d.ProductoFichaTecnicaDetalleId);

            var idsDto = new HashSet<int>(dtoDetalles.Where(d => d.ProductoFichaTecnicaDetalleId > 0)
                                                     .Select(d => d.ProductoFichaTecnicaDetalleId));

            // 1) UPDATE existentes
            foreach (var dto in dtoDetalles.Where(d => d.ProductoFichaTecnicaDetalleId > 0))
            {
                if (actualesPorId.TryGetValue(dto.ProductoFichaTecnicaDetalleId, out var det))
                {
                    // Map sobre entidad existente (trackeada)
                    det.Dato = dto.Dato;
                    det.Descripcion = dto.Descripcion;
                    det.Activo = dto.Activo;
                    det.DetalleListaFichaTecnicaId = dto.DetalleListaFichaTecnicaId;
                    // det.ProductoFichaTecnicaId lo mantiene EF
                }
                else
                {
                    // (raro) vino id que no está cargado; lo agregamos como attach+modified o lo ignoramos.
                    // Aquí preferimos crear/adjuntar nuevo para robustez mínima:
                    entity.ProductoFichaTecnicaDetalles.Add(new ProductoFichaTecnicaDetalle
                    {
                        ProductoFichaTecnicaDetalleId = dto.ProductoFichaTecnicaDetalleId,
                        Dato = dto.Dato,
                        Descripcion = dto.Descripcion,
                        Activo = dto.Activo,
                        DetalleListaFichaTecnicaId = dto.DetalleListaFichaTecnicaId
                    });
                }
            }

            // 2) INSERT nuevos (id == 0)
            foreach (var dto in dtoDetalles.Where(d => d.ProductoFichaTecnicaDetalleId <= 0))
            {
                entity.ProductoFichaTecnicaDetalles.Add(new ProductoFichaTecnicaDetalle
                {
                    // sin Id -> nuevo
                    Dato = dto.Dato,
                    Descripcion = dto.Descripcion,
                    Activo = dto.Activo,
                    DetalleListaFichaTecnicaId = dto.DetalleListaFichaTecnicaId
                });
            }

            // 3) DELETE removidos (los que están en BD pero no vienen en el DTO)
            var aEliminar = entity.ProductoFichaTecnicaDetalles
                .Where(d => d.ProductoFichaTecnicaDetalleId > 0 && !idsDto.Contains(d.ProductoFichaTecnicaDetalleId))
                .ToList();

            foreach (var det in aEliminar)
                entity.ProductoFichaTecnicaDetalles.Remove(det);
        }




        private bool LgProductoFichaTecnicaExists(int id)
        {
            return _context.LgProductoFichaTecnica.Any(e => e.ProductoFichaTecnicaId == id);
        }
    }
}
