using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_intiSoft.Models.Logistica.Producto;
using intiSoft;
using System.Data.Common;
using System.Data;
using api_intiSoft.Dto.Logistica.Producto;
using Npgsql;
using System.ComponentModel.DataAnnotations;
using NpgsqlTypes;

namespace api_intiSoft.Controllers.Logistica.Producto
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoesController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public ProductoesController(ConecDinamicaContext context)
        {
            _context = context;
        }

        //Get Solo devuleve el nombre primer producto encontrado por categoria_id
        [HttpGet("GetProductoByCategoriaId/{categoria_id}")]
        public async Task<ActionResult<string>> GetProductoByCategoriaId(int categoria_id)
        {
            //devuelve solo el nombre del producto
            var producto = await _context.Producto
                .Where(p => p.CategoriaId == categoria_id)
                .Select(p => new 
                {
                    Nombre = p.Nombre
                })
                .FirstOrDefaultAsync();

            return Ok(producto);

        }

        // GET: api/Productoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Logistica.Producto.Producto>>> GetProducto()
        {
            return await _context.Producto.ToListAsync();
        }

        // GET: api/Productoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Logistica.Producto.Producto>> GetProducto(int id)
        {
            var Producto = await _context.Producto.FindAsync(id);
            if (Producto == null)
            {
                return NotFound();
            }
            return Producto;
        }


        /*_threshold
            Su valor va entre 0 y 1:
                0.0 → acepta todo (sin filtro).
                1.0 → exige coincidencia exacta.

                0.30 (30%) → significa que solo se devuelven coincidencias con al menos un 30% de similitud/relevancia.similitud baja (≈ 0.3, justo en el límite).
                0.50 similitud media (≈ 0.5)
                0.90 similitud alta (≈ 0.9)
         */        

        [HttpGet("searchProducto")]
        public async Task<ActionResult<IReadOnlyList<ProductSearchResultDto>>> SearchAsync(
        [FromQuery, Required] string _q,
        [FromQuery] string? _exclude = null,
        [FromQuery] bool? _activo = null, // true: solo activos, false: solo inactivos, null: todos
        [FromQuery] int _limit = 50,
        [FromQuery] int _offset = 0,
        [FromQuery] bool _noFuzzy = false,
        CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(_q))
                return Ok(Array.Empty<ProductSearchResultDto>());

            _limit = Math.Clamp(_limit, 1, 200);
            _offset = Math.Max(0, _offset);

            // Parámetros para controlar fuzzy (coherentes con la función SQL)
            var p_min_ft_hits = _noFuzzy ? 1_000_000 : 20;
            var p_min_len = _noFuzzy ? 1_000 : 3;
            var p_fts_w = 1.0m;
            var p_tri_w = _noFuzzy ? 0.0m : 0.30m;

            const string sql = @"
            SELECT *
            FROM public.search_pvp(
              @p_q,                -- texto a buscar
              @p_exclude,          -- exclusiones
              @p_activo,
              @p_limit,
              @p_offset,
              @p_min_ft_hits,
              @p_min_len,
              @p_fts_w,
              @p_tri_w
            );
            ";

            await using var conn = (NpgsqlConnection)_context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync(ct);

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("p_q", NpgsqlTypes.NpgsqlDbType.Text, _q);
            cmd.Parameters.AddWithValue("p_exclude", NpgsqlTypes.NpgsqlDbType.Text, (object?)_exclude ?? DBNull.Value);

            // Para tri-estado, definimos el parámetro con tipo y valor NULL si corresponde
            var pActivo = new NpgsqlParameter("p_activo", NpgsqlDbType.Boolean)
            {
                Value = (object?)_activo ?? DBNull.Value
            };
            cmd.Parameters.Add(pActivo);

            cmd.Parameters.AddWithValue("p_limit", NpgsqlTypes.NpgsqlDbType.Integer, _limit);
            cmd.Parameters.AddWithValue("p_offset", NpgsqlTypes.NpgsqlDbType.Integer, _offset);
            cmd.Parameters.AddWithValue("p_min_ft_hits", NpgsqlTypes.NpgsqlDbType.Integer, p_min_ft_hits);
            cmd.Parameters.AddWithValue("p_min_len", NpgsqlTypes.NpgsqlDbType.Integer, p_min_len);
            cmd.Parameters.AddWithValue("p_fts_w", NpgsqlTypes.NpgsqlDbType.Numeric, p_fts_w);
            cmd.Parameters.AddWithValue("p_tri_w", NpgsqlTypes.NpgsqlDbType.Numeric, p_tri_w);

            var results = new List<ProductSearchResultDto>(_limit);

            // Helper local para ordinal (intenta nombre tal cual y en minúsculas)
            static int Ord(IDataRecord r, string name)
            {
                try { return r.GetOrdinal(name); }
                catch (IndexOutOfRangeException)
                {
                    return r.GetOrdinal(name.ToLowerInvariant());
                }
            }

            //await using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SequentialAccess, ct);
            await using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SequentialAccess, ct);
            results = await ReadResults(reader, ct);
            return Ok(results);
            /*
            // Calculamos ordinals una sola vez (mejor rendimiento)
            int o_nombreProducto = -1;
            int o_descripcionProducto = -1;
            int o_categoria = -1;
            int o_marca = -1;
            int o_modelo = -1;
            int o_nombreVariante = -1;
            int o_nombrePresentacion = -1;
            int o_descripcionPresentacion = -1;
            int o_orden = -1;
            int o_nombre_grupo = -1;
            int o_numeracion_grupo = -1;
            int o_nombre_compuesto_completo = -1;
            int o_nombre_compuesto_corto = -1;
            int o_sku = -1;
            int o_producto_id = -1;
            int o_marca_id = -1;
            int o_categoria_id = -1;
            int o_producto_presentacion_id = -1;
            int o_producto_variante_presentacion_id = -1;
            int o_activoProducto = -1;
            int o_activoPresentacion = -1;
            int o_activoVariantePresentacion = -1;
            int o_score = -1;

            if (reader.FieldCount > 0)
            {
                o_nombreProducto              = Ord(reader, "nombreProducto");
                o_descripcionProducto         = Ord(reader, "descripcionProducto");
                o_categoria                   = Ord(reader, "categoria");
                o_marca                       = Ord(reader, "marca");
                o_modelo                      = Ord(reader, "modelo");
                o_nombreVariante              = Ord(reader, "nombreVariante");
                o_nombrePresentacion          = Ord(reader, "nombrePresentacion");
                o_descripcionPresentacion     = Ord(reader, "descripcionPresentacion");
                o_orden                       = Ord(reader, "orden");
                o_nombre_grupo                = Ord(reader, "nombre_grupo");
                o_numeracion_grupo            = Ord(reader, "numeracion_grupo");
                o_nombre_compuesto_completo   = Ord(reader, "nombre_compuesto_completo");
                o_nombre_compuesto_corto      = Ord(reader, "nombre_compuesto_corto");
                o_sku                         = Ord(reader, "sku");
                o_producto_id                 = Ord(reader, "producto_id");
                o_marca_id                    = Ord(reader, "marca_id");
                o_categoria_id                = Ord(reader, "categoria_id");
                o_producto_presentacion_id    = Ord(reader, "producto_presentacion_id");
                o_producto_variante_presentacion_id = Ord(reader, "producto_variante_presentacion_id");
                o_activoProducto              = Ord(reader, "activoProducto");
                o_activoPresentacion          = Ord(reader, "activoPresentacion");
                o_activoVariantePresentacion  = Ord(reader, "activoVariantePresentacion");
                o_score                       = Ord(reader, "score");
            }

            while (await reader.ReadAsync(ct))
            {
                var dto = new ProductSearchResultDto
                {
                    NombreProducto             = reader.IsDBNull(o_nombreProducto) ? null : reader.GetString(o_nombreProducto),
                    DescripcionProducto        = reader.IsDBNull(o_descripcionProducto) ? null : reader.GetString(o_descripcionProducto),
                    Categoria                  = reader.IsDBNull(o_categoria) ? null : reader.GetString(o_categoria),
                    Marca                      = reader.IsDBNull(o_marca) ? null : reader.GetString(o_marca),
                    Modelo                     = reader.IsDBNull(o_modelo) ? null : reader.GetString(o_modelo),
                    NombreVariante             = reader.IsDBNull(o_nombreVariante) ? null : reader.GetString(o_nombreVariante),
                    NombrePresentacion         = reader.IsDBNull(o_nombrePresentacion) ? null : reader.GetString(o_nombrePresentacion),
                    DescripcionPresentacion    = reader.IsDBNull(o_descripcionPresentacion) ? null : reader.GetString(o_descripcionPresentacion),
                    Orden                      = reader.IsDBNull(o_orden) ? null : reader.GetInt32(o_orden),
                    Nombre_Grupo               = reader.IsDBNull(o_nombre_grupo) ? null : reader.GetString(o_nombre_grupo),
                    Numeracion_Grupo           = reader.IsDBNull(o_numeracion_grupo) ? null : reader.GetInt32(o_numeracion_grupo),
                    NombreCompuestoCompleto  = reader.IsDBNull(o_nombre_compuesto_completo) ? null : reader.GetString(o_nombre_compuesto_completo),
                    NombreCompuestoCorto     = reader.IsDBNull(o_nombre_compuesto_corto) ? null : reader.GetString(o_nombre_compuesto_corto),
                    Sku                        = reader.IsDBNull(o_sku) ? null : reader.GetString(o_sku),
                    ProductoId                = reader.GetInt32(o_producto_id),
                    MarcaId                   = reader.IsDBNull(o_marca_id) ? null : reader.GetInt32(o_marca_id),
                    CategoriaId               = reader.IsDBNull(o_categoria_id) ? null : reader.GetInt32(o_categoria_id),
                    ProductoPresentacionId   = reader.IsDBNull(o_producto_presentacion_id) ? null : reader.GetInt32(o_producto_presentacion_id),
                    ProductoVariantePresentacionId = reader.GetInt32(o_producto_variante_presentacion_id),
                    ActivoProducto             = reader.GetBoolean(o_activoProducto),
                    ActivoPresentacion         = reader.GetBoolean(o_activoPresentacion),
                    ActivoVariantePresentacion = reader.GetBoolean(o_activoVariantePresentacion),
                    Score                      = reader.GetDecimal(o_score)
                };

                results.Add(dto);
            }
            return Ok(results);*/
        }


        // ============ SKU ============
        [HttpGet("searchBySku")]
        public async Task<ActionResult<IReadOnlyList<ProductSearchResultDto>>> SearchBySkuAsync(
            [FromQuery, Required] string _sku,
            [FromQuery] bool _prefix = false,
            [FromQuery] bool? _activo = null,
            [FromQuery] int _limit = 50,
            [FromQuery] int _offset = 0,
            CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(_sku))
                return Ok(Array.Empty<ProductSearchResultDto>());

            _limit = Math.Clamp(_limit, 1, 200);
            _offset = Math.Max(0, _offset);

            const string sql = @"
            SELECT *, 1.0::numeric AS score
            FROM public.search_by_sku(
              @p_sku,
              @p_prefix,
              @p_activo,
              @p_limit,
              @p_offset
            );";

            await using var conn = (NpgsqlConnection)_context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync(ct);

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("p_sku", NpgsqlDbType.Text, _sku);
            cmd.Parameters.AddWithValue("p_prefix", NpgsqlDbType.Boolean, _prefix);
            cmd.Parameters.Add(new NpgsqlParameter("p_activo", NpgsqlDbType.Boolean) { Value = (object?)_activo ?? DBNull.Value });
            cmd.Parameters.AddWithValue("p_limit", NpgsqlDbType.Integer, _limit);
            cmd.Parameters.AddWithValue("p_offset", NpgsqlDbType.Integer, _offset);

            await using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SequentialAccess, ct);
            var results = await ReadResults(reader, ct);
            return Ok(results);
        }

        // ============ CÓDIGO INTERNO ============
        [HttpGet("searchByCodigoInterno")]
        public async Task<ActionResult<IReadOnlyList<ProductSearchResultDto>>> SearchByCodigoInternoAsync(
            [FromQuery, Required] string _codigo,
            [FromQuery] bool _prefix = false,
            [FromQuery] bool? _activo = null,
            [FromQuery] int _limit = 50,
            [FromQuery] int _offset = 0,
            CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(_codigo))
                return Ok(Array.Empty<ProductSearchResultDto>());

            _limit = Math.Clamp(_limit, 1, 200);
            _offset = Math.Max(0, _offset);

            const string sql = @"
            SELECT *, 1.0::numeric AS score
            FROM public.search_by_codigo_interno(
              @p_codigo,
              @p_prefix,
              @p_activo,
              @p_limit,
              @p_offset
            );";

            await using var conn = (NpgsqlConnection)_context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync(ct);

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("p_codigo", NpgsqlDbType.Text, _codigo);
            cmd.Parameters.AddWithValue("p_prefix", NpgsqlDbType.Boolean, _prefix);
            cmd.Parameters.Add(new NpgsqlParameter("p_activo", NpgsqlDbType.Boolean) { Value = (object?)_activo ?? DBNull.Value });
            cmd.Parameters.AddWithValue("p_limit", NpgsqlDbType.Integer, _limit);
            cmd.Parameters.AddWithValue("p_offset", NpgsqlDbType.Integer, _offset);

            await using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SequentialAccess, ct);
            var results = await ReadResults(reader, ct);
            return Ok(results);
        }

        // ============ CÓDIGO DE BARRA ============
        [HttpGet("searchByCodigoBarra")]
        public async Task<ActionResult<IReadOnlyList<ProductSearchResultDto>>> SearchByCodigoBarraAsync(
            [FromQuery, Required] string _codigo,
            [FromQuery] bool _prefix = false,
            [FromQuery] bool? _activo = null,
            [FromQuery] int _limit = 50,
            [FromQuery] int _offset = 0,
            CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(_codigo))
                return Ok(Array.Empty<ProductSearchResultDto>());

            _limit = Math.Clamp(_limit, 1, 200);
            _offset = Math.Max(0, _offset);

            const string sql = @"
            SELECT *, 1.0::numeric AS score
            FROM public.search_by_codigo_barra(
              @p_codigo,
              @p_prefix,
              @p_activo,
              @p_limit,
              @p_offset
            );";

            await using var conn = (NpgsqlConnection)_context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync(ct);

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("p_codigo", NpgsqlDbType.Text, _codigo);
            cmd.Parameters.AddWithValue("p_prefix", NpgsqlDbType.Boolean, _prefix);
            cmd.Parameters.Add(new NpgsqlParameter("p_activo", NpgsqlDbType.Boolean) { Value = (object?)_activo ?? DBNull.Value });
            cmd.Parameters.AddWithValue("p_limit", NpgsqlDbType.Integer, _limit);
            cmd.Parameters.AddWithValue("p_offset", NpgsqlDbType.Integer, _offset);

            await using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SequentialAccess, ct);
            var results = await ReadResults(reader, ct);
            return Ok(results);
        }

        // ============ CÓDIGO QR (solo exacto) ============
        [HttpGet("searchByCodigoQr")]
        public async Task<ActionResult<IReadOnlyList<ProductSearchResultDto>>> SearchByCodigoQrAsync(
            [FromQuery, Required] string _qr,
            [FromQuery] bool? _activo = null,
            [FromQuery] int _limit = 50,
            [FromQuery] int _offset = 0,
            CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(_qr))
                return Ok(Array.Empty<ProductSearchResultDto>());

            _limit = Math.Clamp(_limit, 1, 200);
            _offset = Math.Max(0, _offset);

            const string sql = @"
                SELECT *, 1.0::numeric AS score
                FROM public.search_by_codigo_qr(
                  @p_qr,
                  @p_activo,
                  @p_limit,
                  @p_offset
                );";

            await using var conn = (NpgsqlConnection)_context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync(ct);

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("p_qr", NpgsqlDbType.Text, _qr);
            cmd.Parameters.Add(new NpgsqlParameter("p_activo", NpgsqlDbType.Boolean) { Value = (object?)_activo ?? DBNull.Value });
            cmd.Parameters.AddWithValue("p_limit", NpgsqlDbType.Integer, _limit);
            cmd.Parameters.AddWithValue("p_offset", NpgsqlDbType.Integer, _offset);

            await using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SequentialAccess, ct);
            var results = await ReadResults(reader, ct);
            return Ok(results);
        }

        // ============ BÚSQUEDA POR PREFIJO (SIN FUZY) ============
        [HttpGet("searchProductoPrefix")]
        public async Task<ActionResult<IReadOnlyList<ProductSearchResultDto>>> SearchProductoPrefixAsync(
           [FromQuery, Required] string _q,
           [FromQuery] int _limit = 50,
           [FromQuery] int _offset = 0,
           [FromQuery] bool? _activo = null,
           CancellationToken ct = default)
        {
            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(_q))
                return Ok(Array.Empty<ProductSearchResultDto>());

            _limit = Math.Clamp(_limit, 1, 200);
            _offset = Math.Max(0, _offset);

            const string sql = @"
                SELECT *
                FROM public.search_pvp_prefix(
                    @p_q,
                    @p_limit,
                    @p_offset,
                    @p_activo
                );
            ";

            await using var conn = (NpgsqlConnection)_context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync(ct);

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("p_q", NpgsqlDbType.Text, _q);
            cmd.Parameters.AddWithValue("p_limit", NpgsqlDbType.Integer, _limit);
            cmd.Parameters.AddWithValue("p_offset", NpgsqlDbType.Integer, _offset);

            // Tri-estado de _activo: si es null enviamos DBNull
            var pActivo = new NpgsqlParameter("p_activo", NpgsqlDbType.Boolean)
            {
                Value = (object?)_activo ?? DBNull.Value
            };
            cmd.Parameters.Add(pActivo);

            var results = new List<ProductSearchResultDto>(_limit);

            static int Ord(IDataRecord r, string name)
            {
                try { return r.GetOrdinal(name); }
                catch (IndexOutOfRangeException) { return r.GetOrdinal(name.ToLowerInvariant()); }
            }

            await using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SequentialAccess, ct);
            
            results = await ReadResults(reader, ct);          


            return Ok(results);
        }

        //
        [HttpGet("searchProductoPrefixgrouped")]
        public async Task<ActionResult<IReadOnlyList<ProductSearchResultDto>>> SearchProductoPrefixgroupedAsync(
           [FromQuery, Required] string _q,
           [FromQuery] int _limit = 50,
           [FromQuery] int _offset = 0,
           [FromQuery] bool? _activo = null,
           CancellationToken ct = default)
        {
            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(_q))
                return Ok(Array.Empty<ProductSearchResultDto>());

            _limit = Math.Clamp(_limit, 1, 200);
            _offset = Math.Max(0, _offset);

            const string sql = @"
                SELECT *
                FROM public.search_pvp_prefix_grouped(
                    @p_q,
                    @p_limit,
                    @p_offset,
                    @p_activo
                );
            ";

            await using var conn = (NpgsqlConnection)_context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync(ct);

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("p_q", NpgsqlDbType.Text, _q);
            cmd.Parameters.AddWithValue("p_limit", NpgsqlDbType.Integer, _limit);
            cmd.Parameters.AddWithValue("p_offset", NpgsqlDbType.Integer, _offset);

            // Tri-estado de _activo: si es null enviamos DBNull
            var pActivo = new NpgsqlParameter("p_activo", NpgsqlDbType.Boolean)
            {
                Value = (object?)_activo ?? DBNull.Value
            };
            cmd.Parameters.Add(pActivo);

            var results = new List<ProductSearchResultDto>(_limit);

            static int Ord(IDataRecord r, string name)
            {
                try { return r.GetOrdinal(name); }
                catch (IndexOutOfRangeException) { return r.GetOrdinal(name.ToLowerInvariant()); }
            }

            await using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SequentialAccess, ct);

            results = await ReadResults(reader, ct);


            return Ok(results);
        }

        // ================== Helpers comunes ==================
       

        // ============ BÚSQUEDA "CONTIENE EN CUALQUIER PARTE" (TRIGRAM + WORD_SIMILARITY) ============
        [HttpGet("searchProductoContains")]
        public async Task<ActionResult<IReadOnlyList<ProductSearchResultDto>>> SearchProductoContainsAsync(
           [FromQuery, Required] string _q,
           [FromQuery] int _limit = 50,
           [FromQuery] int _offset = 0,
           [FromQuery] bool? _activo = null,
           [FromQuery] float _threshold = 0.10f,     // 10%
           [FromQuery] bool _allTokens = false,      // true: AND, false: OR
           CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(_q))
                return Ok(Array.Empty<ProductSearchResultDto>());

            _limit = Math.Clamp(_limit, 1, 200);
            _offset = Math.Max(0, _offset);
            _threshold = Math.Clamp(_threshold, 0.0f, 1.0f);

            if (Request.Query.TryGetValue("_activo", out var raw) && string.Equals(raw, "null", StringComparison.OrdinalIgnoreCase))
                _activo = null;

            const string sql = @"
                SELECT *
                FROM public.search_pvp_contains(
                    @p_q,
                    @p_limit,
                    @p_offset,
                    @p_activo,
                    @p_threshold,
                    @p_alltokens
                );";

            await using var conn = (NpgsqlConnection)_context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync(ct);

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("p_q", NpgsqlDbType.Text, _q);
            cmd.Parameters.AddWithValue("p_limit", NpgsqlDbType.Integer, _limit);
            cmd.Parameters.AddWithValue("p_offset", NpgsqlDbType.Integer, _offset);
            var pActivo = new NpgsqlParameter("p_activo", NpgsqlDbType.Boolean) { Value = (object?)_activo ?? DBNull.Value };
            cmd.Parameters.Add(pActivo);
            cmd.Parameters.AddWithValue("p_threshold", NpgsqlDbType.Real, _threshold);
            cmd.Parameters.AddWithValue("p_alltokens", NpgsqlDbType.Boolean, _allTokens);

            await using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SequentialAccess, ct);
            var results = await ReadResults(reader, ct);
            return Ok(results);
        }

        // ============ BÚSQUEDA "CONTIENE" AGRUPADA POR PRODUCTO ============
        [HttpGet("searchProductoContainsgrouped")]
        public async Task<ActionResult<IReadOnlyList<ProductSearchResultDto>>> SearchProductoContainsgroupedAsync(
           [FromQuery, Required] string _q,
           [FromQuery] int _limit = 50,
           [FromQuery] int _offset = 0,
           [FromQuery] bool? _activo = null,
           [FromQuery] float _threshold = 0.10f,
           [FromQuery] bool _allTokens = false,
           CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(_q))
                return Ok(Array.Empty<ProductSearchResultDto>());

            _limit = Math.Clamp(_limit, 1, 200);
            _offset = Math.Max(0, _offset);
            _threshold = Math.Clamp(_threshold, 0.0f, 1.0f);

            if (Request.Query.TryGetValue("_activo", out var raw) && string.Equals(raw, "null", StringComparison.OrdinalIgnoreCase))
                _activo = null;

            const string sql = @"
                SELECT *
                FROM public.search_pvp_contains_grouped(
                    @p_q,
                    @p_limit,
                    @p_offset,
                    @p_activo,
                    @p_threshold,
                    @p_alltokens
                );";

            await using var conn = (NpgsqlConnection)_context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync(ct);

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("p_q", NpgsqlDbType.Text, _q);
            cmd.Parameters.AddWithValue("p_limit", NpgsqlDbType.Integer, _limit);
            cmd.Parameters.AddWithValue("p_offset", NpgsqlDbType.Integer, _offset);
            var pActivo = new NpgsqlParameter("p_activo", NpgsqlDbType.Boolean) { Value = (object?)_activo ?? DBNull.Value };
            cmd.Parameters.Add(pActivo);
            cmd.Parameters.AddWithValue("p_threshold", NpgsqlDbType.Real, _threshold);
            cmd.Parameters.AddWithValue("p_alltokens", NpgsqlDbType.Boolean, _allTokens);

            await using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SequentialAccess, ct);
            var results = await ReadResults(reader, ct);
            return Ok(results);
        }


        // ======================= ANYWHERE (no agrupado) =======================
        [HttpGet("searchProductoAnywhere")]
        public async Task<ActionResult<IReadOnlyList<ProductSearchResultDto>>> SearchProductoAnywhereAsync(
           [FromQuery, Required] string _q,
           [FromQuery] int _limit = 50,
           [FromQuery] int _offset = 0,
           [FromQuery] bool? _activo = null,        // TRUE: solo activos, FALSE: solo inactivos, NULL: todos
           [FromQuery] float _threshold = 0.10f,    // 0..1 (aplica a ranking trigram/word_similarity)
           CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(_q))
                return Ok(Array.Empty<ProductSearchResultDto>());

            _limit = Math.Clamp(_limit, 1, 200);
            _offset = Math.Max(0, _offset);
            _threshold = Math.Clamp(_threshold, 0.0f, 1.0f);

            // Evita 400 si llega como string "null"
            if (Request.Query.TryGetValue("_activo", out var raw) &&
                string.Equals(raw, "null", StringComparison.OrdinalIgnoreCase))
            {
                _activo = null;
            }

            const string sql = @"
                SELECT *
                FROM productos.search_pvp_anywhere(
                    @p_q,
                    @p_limit,
                    @p_offset,
                    @p_activo,
                    @p_threshold
                );";

            await using var conn = (NpgsqlConnection)_context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync(ct);

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("p_q", NpgsqlDbType.Text, _q);
            cmd.Parameters.AddWithValue("p_limit", NpgsqlDbType.Integer, _limit);
            cmd.Parameters.AddWithValue("p_offset", NpgsqlDbType.Integer, _offset);

            var pActivo = new NpgsqlParameter("p_activo", NpgsqlDbType.Boolean)
            {
                Value = (object?)_activo ?? DBNull.Value
            };
            cmd.Parameters.Add(pActivo);

            cmd.Parameters.AddWithValue("p_threshold", NpgsqlDbType.Real, _threshold);

            await using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SequentialAccess, ct);
            var results = await ReadResults(reader, ct);
            return Ok(results);
        }

        // =================== ANYWHERE AGRUPADO POR PRODUCTO ===================
        [HttpGet("searchProductoAnywhereGrouped")]
        public async Task<ActionResult<IReadOnlyList<ProductSearchResultDto>>> SearchProductoAnywhereGroupedAsync(
           [FromQuery, Required] string _q,
           [FromQuery] int _limit = 50,
           [FromQuery] int _offset = 0,
           [FromQuery] bool? _activo = null,
           [FromQuery] float _threshold = 0.10f,
           CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(_q))
                return Ok(Array.Empty<ProductSearchResultDto>());

            _limit = Math.Clamp(_limit, 1, 200);
            _offset = Math.Max(0, _offset);
            _threshold = Math.Clamp(_threshold, 0.0f, 1.0f);

            if (Request.Query.TryGetValue("_activo", out var raw) &&
                string.Equals(raw, "null", StringComparison.OrdinalIgnoreCase))
            {
                _activo = null;
            }

            const string sql = @"
                SELECT *
                FROM productos.search_pvp_anywhere_grouped(
                    @p_q,
                    @p_limit,
                    @p_offset,
                    @p_activo,
                    @p_threshold
                );";

            await using var conn = (NpgsqlConnection)_context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync(ct);

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("p_q", NpgsqlDbType.Text, _q);
            cmd.Parameters.AddWithValue("p_limit", NpgsqlDbType.Integer, _limit);
            cmd.Parameters.AddWithValue("p_offset", NpgsqlDbType.Integer, _offset);

            var pActivo = new NpgsqlParameter("p_activo", NpgsqlDbType.Boolean)
            {
                Value = (object?)_activo ?? DBNull.Value
            };
            cmd.Parameters.Add(pActivo);

            cmd.Parameters.AddWithValue("p_threshold", NpgsqlDbType.Real, _threshold);

            await using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SequentialAccess, ct);
            var results = await ReadResults(reader, ct);
            return Ok(results);
        }


        //fulltext
        // ==========================================================
        // 🔹 Búsqueda con Full Text Search AGRUPADO por producto_id
        // ==========================================================
        [HttpGet("searchProductoFulltextGrouped")]
        public async Task<ActionResult<IReadOnlyList<ProductSearchResultDto>>> SearchProductoFulltextGroupedAsync(
            [FromQuery, Required] string _q,
            [FromQuery] int _limit = 50,
            [FromQuery] int _offset = 0,
            [FromQuery] bool? _activo = null,
            CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(_q))
                return Ok(Array.Empty<ProductSearchResultDto>());

            _limit = Math.Clamp(_limit, 1, 200);
            _offset = Math.Max(0, _offset);

            if (Request.Query.TryGetValue("_activo", out var raw) &&
                string.Equals(raw, "null", StringComparison.OrdinalIgnoreCase))
            {
                _activo = null;
            }

            const string sql = @"
                SELECT *
                FROM productos.search_pvp_fulltext_grouped(
                    @p_q,
                    @p_limit,
                    @p_offset,
                    @p_activo
                );";

            await using var conn = (NpgsqlConnection)_context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync(ct);

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("p_q", NpgsqlDbType.Text, _q);
            cmd.Parameters.AddWithValue("p_limit", NpgsqlDbType.Integer, _limit);
            cmd.Parameters.AddWithValue("p_offset", NpgsqlDbType.Integer, _offset);

            var pActivo = new NpgsqlParameter("p_activo", NpgsqlDbType.Boolean)
            {
                Value = (object?)_activo ?? DBNull.Value
            };
            cmd.Parameters.Add(pActivo);

            await using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SequentialAccess, ct);
            var results = await ReadResults(reader, ct);
            return Ok(results);
        }

        // ==========================================================
        // 🔹 Búsqueda con Full Text Search (sin agrupar)
        // ==========================================================
        [HttpGet("searchProductoFulltext")]
        public async Task<ActionResult<IReadOnlyList<ProductSearchResultDto>>> SearchProductoFulltextAsync(
            [FromQuery, Required] string _q,
            [FromQuery] int _limit = 50,
            [FromQuery] int _offset = 0,
            [FromQuery] bool? _activo = null,
            CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(_q))
                return Ok(Array.Empty<ProductSearchResultDto>());

            _limit = Math.Clamp(_limit, 1, 200);
            _offset = Math.Max(0, _offset);

            if (Request.Query.TryGetValue("_activo", out var raw) &&
                string.Equals(raw, "null", StringComparison.OrdinalIgnoreCase))
            {
                _activo = null;
            }

            const string sql = @"
                SELECT *
                FROM productos.search_pvp_fulltext(
                    @p_q,
                    @p_limit,
                    @p_offset,
                    @p_activo
                );";

            await using var conn = (NpgsqlConnection)_context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync(ct);

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("p_q", NpgsqlDbType.Text, _q);
            cmd.Parameters.AddWithValue("p_limit", NpgsqlDbType.Integer, _limit);
            cmd.Parameters.AddWithValue("p_offset", NpgsqlDbType.Integer, _offset);

            var pActivo = new NpgsqlParameter("p_activo", NpgsqlDbType.Boolean)
            {
                Value = (object?)_activo ?? DBNull.Value
            };
            cmd.Parameters.Add(pActivo);

            await using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SequentialAccess, ct);
            var results = await ReadResults(reader, ct);
            return Ok(results);
        }
        //fin de fulltext


        private static int Ord(IDataRecord r, string name)
        {
            try { return r.GetOrdinal(name); }
            catch (IndexOutOfRangeException) { return r.GetOrdinal(name.ToLowerInvariant()); }
        }

        private static async Task<List<ProductSearchResultDto>> ReadResults(DbDataReader reader, CancellationToken ct)
        {
            var results = new List<ProductSearchResultDto>();

            // Ordinals (una sola vez)
            int o_nombreProducto = Ord(reader, "nombreProducto");
            int o_descripcionProducto = Ord(reader, "descripcionProducto");
            int o_categoria = Ord(reader, "categoria");
            int o_marca = Ord(reader, "marca");
            int o_modelo = Ord(reader, "modelo");
            int o_nombreVariante = Ord(reader, "nombreVariante");
            int o_nombrePresentacion = Ord(reader, "nombrePresentacion");
            int o_descripcionPresentacion = Ord(reader, "descripcionPresentacion");
            int o_orden = Ord(reader, "orden");
            int o_nombre_grupo = Ord(reader, "nombre_grupo");
            int o_numeracion_grupo = Ord(reader, "numeracion_grupo");
            int o_nombre_compuesto_completo = Ord(reader, "nombre_compuesto_completo");
            int o_nombre_compuesto_corto = Ord(reader, "nombre_compuesto_corto");
            int o_sku = Ord(reader, "sku");

            int o_codigoBarra = Ord(reader, "codigo_Barra");
            int o_codigoQr = Ord(reader, "codigo_Qr");
            int o_codigoInterno = Ord(reader, "codigo_Interno");
            int o_precioCompra = Ord(reader, "precio_Compra");
            int o_precioVentaFinal = Ord(reader, "precio_Venta_Final");
            int o_stock = Ord(reader, "stock");

            int o_producto_id = Ord(reader, "producto_id");
            int o_marca_id = Ord(reader, "marca_id");
            int o_categoria_id = Ord(reader, "categoria_id");
            int o_producto_presentacion_id = Ord(reader, "producto_presentacion_id");
            int o_producto_variante_presentacion_id = Ord(reader, "producto_variante_presentacion_id");
            int o_activoProducto = Ord(reader, "activoProducto");
            int o_activoPresentacion = Ord(reader, "activoPresentacion");
            int o_activoVariantePresentacion = Ord(reader, "activoVariantePresentacion");
            int o_score = Ord(reader, "score");

            while (await reader.ReadAsync(ct))
            {
                var dto = new ProductSearchResultDto
                {
                    NombreProducto            = reader.IsDBNull(o_nombreProducto) ? null : reader.GetString(o_nombreProducto),
                    DescripcionProducto       = reader.IsDBNull(o_descripcionProducto) ? null : reader.GetString(o_descripcionProducto),
                    Categoria                 = reader.IsDBNull(o_categoria) ? null : reader.GetString(o_categoria),
                    Marca                     = reader.IsDBNull(o_marca) ? null : reader.GetString(o_marca),
                    Modelo                    = reader.IsDBNull(o_modelo) ? null : reader.GetString(o_modelo),
                    NombreVariante            = reader.IsDBNull(o_nombreVariante) ? null : reader.GetString(o_nombreVariante),
                    NombrePresentacion        = reader.IsDBNull(o_nombrePresentacion) ? null : reader.GetString(o_nombrePresentacion),
                    DescripcionPresentacion   = reader.IsDBNull(o_descripcionPresentacion) ? null : reader.GetString(o_descripcionPresentacion),
                    Orden                     = reader.IsDBNull(o_orden) ? null : reader.GetInt32(o_orden),
                    NombreGrupo              = reader.IsDBNull(o_nombre_grupo) ? null : reader.GetString(o_nombre_grupo),
                    NumeracionGrupo          = reader.IsDBNull(o_numeracion_grupo) ? null : reader.GetInt32(o_numeracion_grupo),
                    NombreCompuestoCompleto = reader.IsDBNull(o_nombre_compuesto_completo) ? null : reader.GetString(o_nombre_compuesto_completo),
                    NombreCompuestoCorto    = reader.IsDBNull(o_nombre_compuesto_corto) ? null : reader.GetString(o_nombre_compuesto_corto),
                    Sku                       = reader.IsDBNull(o_sku) ? null : reader.GetString(o_sku),
                    CodigoBarra               = reader.IsDBNull(o_codigoBarra) ? null : reader.GetString(o_codigoBarra),
                    CodigoQr                  = reader.IsDBNull(o_codigoQr) ? null : reader.GetString(o_codigoQr),
                    CodigoInterno             = reader.IsDBNull(o_codigoInterno) ? null : reader.GetString(o_codigoInterno),
                    //PrecioCompra puede ser null
                    PrecioCompra              = reader.IsDBNull(o_precioCompra) ? 0 : reader.GetDecimal(o_precioCompra),
                    PrecioVentaFinal          = reader.IsDBNull(o_precioVentaFinal) ? 0 : reader.GetDecimal(o_precioVentaFinal),
                    Stock                     = reader.IsDBNull(o_stock) ? 0 : reader.GetDecimal(o_stock),
                    ProductoId               = reader.GetInt32(o_producto_id),
                    MarcaId                  = reader.IsDBNull(o_marca_id) ? null : reader.GetInt32(o_marca_id),
                    CategoriaId              = reader.IsDBNull(o_categoria_id) ? null : reader.GetInt32(o_categoria_id),
                    ProductoPresentacionId  = reader.IsDBNull(o_producto_presentacion_id) ? null : reader.GetInt32(o_producto_presentacion_id),
                    ProductoVariantePresentacionId = reader.IsDBNull(o_producto_variante_presentacion_id) ? null : reader.GetInt32(o_producto_variante_presentacion_id),
                    ActivoProducto            = reader.GetBoolean(o_activoProducto),

                    ActivoPresentacion        = reader.IsDBNull(o_activoPresentacion) ? null : reader.GetBoolean(o_activoPresentacion),
                    ActivoVariantePresentacion = reader.IsDBNull(o_activoVariantePresentacion) ? null : reader.GetBoolean(o_activoVariantePresentacion),

                    Score                     = reader.GetDecimal(o_score)
                };
                results.Add(dto);
            }

            return results;
        }





        // PUT: api/Productoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(int id, Models.Logistica.Producto.Producto Producto)
        {
            if (id != Producto.ProductoId)
            {
                return BadRequest();
            }

            _context.Entry(Producto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExists(id))
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

        // POST: api/Productoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Models.Logistica.Producto.Producto>> PostProducto(Models.Logistica.Producto.Producto Producto)
        {
            _context.Producto.Add(Producto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProducto", new { id = Producto.ProductoId }, Producto);
        }

        // DELETE: api/Productoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var Producto = await _context.Producto.FindAsync(id);
            if (Producto == null)
            {
                return NotFound();
            }

            _context.Producto.Remove(Producto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductoExists(int id)
        {
            return _context.Producto.Any(e => e.ProductoId == id);
        }
    }
}
