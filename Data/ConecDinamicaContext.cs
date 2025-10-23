using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using api_intiSoft.Models.CentroEstudios;
using api_intiSoft.Models.Common;
using api_intiSoft.Models.Configuracion;
using api_intiSoft.Models.Data;
using api_intiSoft.Models.Seguridad;
using api_intiSoft.Models.Universal;
using Finbuckle.MultiTenant.Abstractions;

namespace intiSoft
{
    public class ConecDinamicaContext : DbContext
    {
        private readonly ITenantInfo? _tenant;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;

        public ConecDinamicaContext(
            DbContextOptions<ConecDinamicaContext> options,
            IWebHostEnvironment env,
            IMultiTenantContextAccessor multiTenantContextAccessor,
            IConfiguration config)
            : base(options)
        {
            _tenant = multiTenantContextAccessor.MultiTenantContext?.TenantInfo;
            _env = env;
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                base.OnConfiguring(optionsBuilder);
                return;
            }

            var connectionString = ResolveConnectionString();

            optionsBuilder.UseNpgsql(connectionString);

            base.OnConfiguring(optionsBuilder);
        }

        private string ResolveConnectionString()
        {
            if (_tenant is CustomTenantInfo customTenant &&
                !string.IsNullOrWhiteSpace(customTenant.ConnectionString))
            {
                return customTenant.ConnectionString;
            }

            if (!string.IsNullOrWhiteSpace(_tenant?.ConnectionString))
            {
                return _tenant.ConnectionString!;
            }

            if (_tenant is not null)
            {
                var tenantConnection = _config.GetConnectionString(_tenant.Identifier);
                if (!string.IsNullOrWhiteSpace(tenantConnection))
                {
                    return tenantConnection;
                }
            }

            foreach (var candidateName in EnumerateFallbackConnectionNames())
            {
                var fallbackConnection = _config.GetConnectionString(candidateName);
                if (!string.IsNullOrWhiteSpace(fallbackConnection))
                {
                    return fallbackConnection;
                }
            }

            var tenantId = _tenant?.Identifier ?? "(predeterminado)";
            throw new InvalidOperationException(
                $"No se pudo determinar una cadena de conexión válida para el inquilino '{tenantId}'. " +
                "Verifique la configuración de ConnectionStrings o envíe el encabezado 'X-Tenant-INTISoft'.");
        }

        private IEnumerable<string> EnumerateFallbackConnectionNames()
        {
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var preferredNames = new[]
            {
                _env.EnvironmentName,
                nameof(ConecDinamicaContext),
                "ConecDinamicaContext",
                "DefaultConnection",
                "Development"
            };

            foreach (var name in preferredNames)
            {
                if (string.IsNullOrWhiteSpace(name) || !seen.Add(name))
                {
                    continue;
                }

                yield return name;
            }

            foreach (var section in _config.GetSection("ConnectionStrings").GetChildren())
            {
                if (string.IsNullOrWhiteSpace(section.Key) ||
                    string.Equals(section.Key, "Tenants", StringComparison.OrdinalIgnoreCase) ||
                    !seen.Add(section.Key))
                {
                    continue;
                }

                yield return section.Key;
            }
        }

        public DbSet<UnTipoDato> UnTipoDato { get; set; } = default!;

        public DbSet<SgUsuario> SgUsuario { get; set; } = default!;

        public DbSet<Sede> Sede { get; set; } = default!;
        public DbSet<Alumno> Alumno { get; set; } = default!;
        public DbSet<AlumnoApoderado> AlumnoApoderado { get; set; } = default!;
        public DbSet<Apoderado> Apoderado { get; set; } = default!;
        public DbSet<Parentesco> Parentesco { get; set; } = default!;
        public DbSet<Colegio> Colegio { get; set; } = default!;
        public DbSet<Universidad> Universidad { get; set; } = default!;
        public DbSet<Ciclo> Ciclo { get; set; } = default!;
        public DbSet<Curso> Curso { get; set; } = default!;
        public DbSet<Especialidad> Especialidad { get; set; } = default!;
        public DbSet<Nivel> Nivel { get; set; } = default!;
        public DbSet<Docente> Docente { get; set; } = default!;
        public DbSet<SeccionCiclo> SeccionCiclo { get; set; } = default!;
        public DbSet<Seccion> Seccion { get; set; } = default!;
        public DbSet<AperturaCiclo> AperturaCiclo { get; set; } = default!;
        public DbSet<AperturaSeccion> AperturaSeccion { get; set; } = default!;
        public DbSet<Carrera> Carrera { get; set; } = default!;
        public DbSet<ConceptoTipo> ConceptoTipo { get; set; } = default!;
        public DbSet<Concepto> Concepto { get; set; } = default!;
        public DbSet<Matricula> Matricula { get; set; } = default!;
        public DbSet<MatriculaItem> MatriculaItem { get; set; } = default!;
        public DbSet<TipoEvaluacion> TipoEvaluacion { get; set; } = default!;
        public DbSet<EvaluacionProgramadum> EvaluacionProgramadum { get; set; } = default!;
        public DbSet<EvaluacionProgramadaSeccion> EvaluacionProgramadaSeccion { get; set; } = default!;
        public DbSet<EvaluacionDetalle> EvaluacionDetalle { get; set; } = default!;
        public DbSet<EvaluacionTipoPreguntum> EvaluacionTipoPreguntum { get; set; } = default!;
        public DbSet<EvaluacionClave> EvaluacionClave { get; set; } = default!;
        public DbSet<Evaluacion> Evaluacion { get; set; } = default!;
        public DbSet<EvaluacionRespuestum> EvaluacionRespuestum { get; set; } = default!;

        //public DbSet<LgCategorium> LgCategorium { get; set; } = default!;
        //public DbSet<api_intiSoft.Models.Logistica.Producto.LgProducto> LgProducto { get; set; } = default!;
    }
}
