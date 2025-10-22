using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using api_intiSoft.Models.Universal;

using Finbuckle.MultiTenant;
using api_intiSoft.Data;
using Finbuckle.MultiTenant.Abstractions;
using api_intiSoft.Models.Configuracion;
using api_intiSoft.Models.Seguridad;
using api_intiSoft.Models.Common;
using api_intiSoft.Models.CentroEstudios;


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
            string? connectionString;

            if (_tenant is null && _env.IsDevelopment())
            {
                // Init/Dev connection string --Development --Tenants
                connectionString = _config.GetConnectionString("Development");
            }
            else
            {
                // Tenant connection string
                connectionString = _config.GetConnectionString(_tenant!.Identifier);
            }

            // UseSqlServer
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
