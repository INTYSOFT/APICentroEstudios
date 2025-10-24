using Finbuckle.MultiTenant.Abstractions;
using Microsoft.EntityFrameworkCore;


//using api_intiSoft.Models.Logistica.Producto;

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


        //public DbSet<LgCategorium> LgCategorium => Set<LgCategorium>();   
        //public DbSet<api_intiSoft.Models.Producto.LgCategorium> LgCategorium { get; set; } = default!;
        //public DbSet<Product> Products => Set<Product>();




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string? connectionString = null;

            if (_tenant is not null)
            {
                connectionString = _config.GetConnectionString(_tenant.Identifier);

                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new InvalidOperationException($"No se encontró la cadena de conexión para el tenant '{_tenant.Identifier}'.");
                }
            }

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                connectionString = _config.GetConnectionString("Development");
            }

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("No se pudo resolver una cadena de conexión. Configure 'Development' o proporcione un tenant válido mediante el encabezado 'X-Tenant-INTISoft'.");
            }

            optionsBuilder.UseNpgsql(connectionString);

            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<api_intiSoft.Models.Universal.UnTipoDato> UnTipoDato { get; set; } = default!;


        public DbSet<api_intiSoft.Models.Seguridad.SgUsuario> SgUsuario { get; set; } = default!;

        public DbSet<api_intiSoft.Models.CentroEstudios.Sede> Sede { get; set; } = default!;
        public DbSet<api_intiSoft.Models.CentroEstudios.Alumno> Alumno { get; set; } = default!;
        public DbSet<api_intiSoft.Models.CentroEstudios.AlumnoApoderado> AlumnoApoderado { get; set; } = default!;
        public DbSet<api_intiSoft.Models.CentroEstudios.Apoderado> Apoderado { get; set; } = default!;
        public DbSet<api_intiSoft.Models.CentroEstudios.Parentesco> Parentesco { get; set; } = default!;
        public DbSet<api_intiSoft.Models.CentroEstudios.Colegio> Colegio { get; set; } = default!;
        public DbSet<api_intiSoft.Models.CentroEstudios.Universidad> Universidad { get; set; } = default!;
        public DbSet<api_intiSoft.Models.CentroEstudios.Ciclo> Ciclo { get; set; } = default!;
        public DbSet<api_intiSoft.Models.CentroEstudios.Curso> Curso { get; set; } = default!;
        public DbSet<api_intiSoft.Models.CentroEstudios.Especialidad> Especialidad { get; set; } = default!;
        public DbSet<api_intiSoft.Models.CentroEstudios.Nivel> Nivel { get; set; } = default!;
        public DbSet<api_intiSoft.Models.CentroEstudios.Docente> Docente { get; set; } = default!;
        public DbSet<api_intiSoft.Models.CentroEstudios.SeccionCiclo> SeccionCiclo { get; set; } = default!;
        public DbSet<api_intiSoft.Models.CentroEstudios.Seccion> Seccion { get; set; } = default!;
        public DbSet<api_intiSoft.Models.CentroEstudios.AperturaCiclo> AperturaCiclo { get; set; } = default!;
        public DbSet<api_intiSoft.Models.CentroEstudios.AperturaSeccion> AperturaSeccion { get; set; } = default!;
        public DbSet<api_intiSoft.Models.CentroEstudios.Carrera> Carrera { get; set; } = default!;
        public DbSet<api_intiSoft.Models.CentroEstudios.ConceptoTipo> ConceptoTipo { get; set; } = default!;
        public DbSet<api_intiSoft.Models.CentroEstudios.Concepto> Concepto { get; set; } = default!;
        public DbSet<api_intiSoft.Models.CentroEstudios.Matricula> Matricula { get; set; } = default!;
        public DbSet<api_intiSoft.Models.CentroEstudios.MatriculaItem> MatriculaItem { get; set; } = default!;
        public DbSet<api_intiSoft.Models.CentroEstudios.TipoEvaluacion> TipoEvaluacion { get; set; } = default!;

        public DbSet<api_intiSoft.Models.CentroEstudios.EvaluacionProgramadum> EvaluacionProgramadum { get; set; } = default!;

        public DbSet<api_intiSoft.Models.CentroEstudios.EvaluacionProgramadaSeccion> EvaluacionProgramadaSeccion { get; set; } = default!;
        public DbSet<api_intiSoft.Models.CentroEstudios.EvaluacionDetalle> EvaluacionDetalle { get; set; } = default!;
        public DbSet<api_intiSoft.Models.CentroEstudios.EvaluacionTipoPreguntum> EvaluacionTipoPreguntum { get; set; } = default!;
        public DbSet<api_intiSoft.Models.CentroEstudios.EvaluacionClave> EvaluacionClave { get; set; } = default!;
        public DbSet<api_intiSoft.Models.CentroEstudios.Evaluacion> Evaluacion { get; set; } = default!;
        public DbSet<api_intiSoft.Models.CentroEstudios.EvaluacionRespuestum> EvaluacionRespuestum { get; set; } = default!;


        //public DbSet<LgCategorium> LgCategorium { get; set; } = default!;
        //public DbSet<api_intiSoft.Models.Logistica.Producto.LgProducto> LgProducto { get; set; } = default!;


    }
}
