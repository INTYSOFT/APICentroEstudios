using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using api_intiSoft.Models.Universal;

using Finbuckle.MultiTenant;
using api_intiSoft.Data;
using api_intiSoft.Models.Logistica.Producto;
using api_intiSoft.Models.Contabilidad;
using Finbuckle.MultiTenant.Abstractions;
using api_intiSoft.Models.Configuracion;
using api_intiSoft.Models.Clinica;
using api_intiSoft.Models.Seguridad;
using api_intiSoft.Models.Compras;
using api_intiSoft.Models.Common;
using api_intiSoft.Configurations;

//using api_intiSoft.Models.Logistica.Producto;

namespace intiSoft
{
    public class ConecDinamicaContext : DbContext
    {


        private readonly ITenantInfo? _tenant;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 👇 Aquí se aplican las configuraciones separadas
            modelBuilder.ApplyConfiguration(new ProductoConfiguration());
            modelBuilder.ApplyConfiguration(new CategoriasConfiguration());            
            modelBuilder.ApplyConfiguration(new ProductoFichaTecnicaConfiguration());            
            modelBuilder.ApplyConfiguration(new ProductoFichaTecnicaDetalleConfiguration());
            modelBuilder.ApplyConfiguration(new UnidadMedidumConfiguration());
            modelBuilder.ApplyConfiguration(new ConversionUnidadConfiguration());            
            modelBuilder.ApplyConfiguration(new FichaTecnicaConfiguration());
            modelBuilder.ApplyConfiguration(new CategoriaFichaTecnicaConfiguration());
            modelBuilder.ApplyConfiguration(new CategoriaFichaTecnicaDetalleConfiguration());
            modelBuilder.ApplyConfiguration(new ProductoVarianteConfiguration());
            modelBuilder.ApplyConfiguration(new ProductoVarianteDetalleConfiguration());
            modelBuilder.ApplyConfiguration(new ProductoVariantePresentacionConfiguration());
            modelBuilder.ApplyConfiguration(new ProductoVariantePresentacionDetConfiguration());
            modelBuilder.ApplyConfiguration(new MarcaConfiguration());
            modelBuilder.ApplyConfiguration(new LineaConfiguration());            
            modelBuilder.ApplyConfiguration(new ListaFichaTecnicaConfiguration());
            modelBuilder.ApplyConfiguration(new DetalleListaFichaTecnicaConfiguration());
            modelBuilder.ApplyConfiguration(new TipoListumConfiguration());
            modelBuilder.ApplyConfiguration(new TipoUsoProductoConfiguration());            
            modelBuilder.ApplyConfiguration(new TipoProductosConfiguration());            
            modelBuilder.ApplyConfiguration(new TipoUnidadMedidumConfiguration());            
            modelBuilder.ApplyConfiguration(new EmpaqueConfiguration());            
            modelBuilder.ApplyConfiguration(new ModeloConfiguration());            
            modelBuilder.ApplyConfiguration(new ProductoServicioConfiguration());
            modelBuilder.ApplyConfiguration(new UnidadMedidumConfiguration());            
            modelBuilder.ApplyConfiguration(new CategoriaFichaTecnicaDetalleConfiguration());            
            modelBuilder.ApplyConfiguration(new ProductoPresentacionConfiguration());

            //modelBuilder.ApplyConfiguration(new CategoriaConfiguration());
            // ...agrega más si tienes otras entidades configuradas por separado
        }

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

        public DbSet<api_intiSoft.Models.Logistica.Producto.Categorias> LgCategorium { get; set; } = default!;
        public DbSet<api_intiSoft.Models.Logistica.Producto.VwCategoriaRutaCompletum> VwCategoriaRutaCompletum { get; set; } = default!;

        public DbSet<api_intiSoft.Models.Logistica.Producto.Producto> Producto { get; set; } = default!;
        
        public DbSet<api_intiSoft.Models.Logistica.Producto.UnidadMedidum> LgUnidadMedidum { get; set; } = default!;
       
        public DbSet<api_intiSoft.Models.Logistica.Producto.ConversionUnidad> LgConversionUnidad { get; set; } = default!;
        public DbSet<api_intiSoft.Models.Contabilidad.CtPlanContable> CtPlanContable { get; set; } = default!;
        
        public DbSet<api_intiSoft.Models.Logistica.Producto.FichaTecnica> LgFichaTecnica { get; set; } = default!;
        public DbSet<api_intiSoft.Models.Logistica.Producto.CategoriaFichaTecnica> LgCategoriaFichaTecnica { get; set; } = default!;   



        public DbSet<api_intiSoft.Models.Logistica.Producto.ProductoFichaTecnica> LgProductoFichaTecnica { get; set; } = default!;
        public DbSet<api_intiSoft.Models.Logistica.Producto.Empaque> LgEmpaque { get; set; } = default!;
        
        public DbSet<api_intiSoft.Models.Universal.UnTipoDato> UnTipoDato { get; set; } = default!;
        public DbSet<api_intiSoft.Models.Logistica.Producto.CategoriaFichaTecnicaDetalle> LgCategoriaFichaTecnicaDetalle { get; set; } = default!;
    
        //LgMarca
        public DbSet<api_intiSoft.Models.Logistica.Producto.Marca> LgMarca { get; set; } = default!;
 
        public DbSet<api_intiSoft.Models.Logistica.Producto.Linea> LgLinea { get; set; } = default!;
        //tipo marca
        public DbSet<api_intiSoft.Models.Logistica.Producto.TipoMarca> LgTipoMarca { get; set; } = default!;
        
        public DbSet<api_intiSoft.Models.Logistica.Producto.ListaFichaTecnica> LgListaFichaTecnica { get; set; } = default!;
        public DbSet<api_intiSoft.Models.Logistica.Producto.DetalleListaFichaTecnica> LgDetalleListaFichaTecnica { get; set; } = default!;
        public DbSet<api_intiSoft.Models.Logistica.Producto.ProductoPresentacion> LgProductoPresentacion { get; set; } = default!;
        public DbSet<api_intiSoft.Models.Configuracion.CfProducto> CfProducto { get; set; } = default!;
        public DbSet<api_intiSoft.Models.Logistica.Producto.TipoProductos> LgTipoProducto { get; set; } = default!;
        public DbSet<api_intiSoft.Models.Logistica.Producto.Modelo> LgModelo { get; set; } = default!;
        public DbSet<api_intiSoft.Models.Logistica.Producto.ProductoServicio> LgProductoServicio { get; set; } = default!;
        public DbSet<api_intiSoft.Models.Logistica.Producto.ProductoVariante> LgProductoVariante { get; set; } = default!;
        public DbSet<api_intiSoft.Models.Clinica.LgCliente> GmCitaMedica { get; set; } = default!;
        public DbSet<api_intiSoft.Models.Clinica.GmEspecialidad> GmEspecialidad { get; set; } = default!;
        public DbSet<api_intiSoft.Models.Clinica.GmEstadoCitum> GmEstadoCitum { get; set; } = default!;
        public DbSet<api_intiSoft.Models.Clinica.GmHistoriaClinica> GmHistoriaClinica { get; set; } = default!;
        public DbSet<api_intiSoft.Models.Clinica.GmMedico> GmMedico { get; set; } = default!;
        public DbSet<api_intiSoft.Models.Clinica.GmTipoEventoClinico> GmTipoEventoClinico { get; set; } = default!;
        public DbSet<api_intiSoft.Models.Clinica.GmHistoriaClinicaEvento> GmHistoriaClinicaEvento { get; set; } = default!;
        public DbSet<LgDocumentoIdentidad> LgDocumentoIdentidad { get; set; } = default!;
        
        
        public DbSet<api_intiSoft.Models.Clinica.GmHorarioMedico> GmHorarioMedico { get; set; } = default!;
        public DbSet<api_intiSoft.Models.Clinica.GmTipoHorario> GmTipoHorario { get; set; } = default!;
        
        public DbSet<api_intiSoft.Models.Clinica.GmHorarioApertura> GmHorarioApertura { get; set; } = default!;
        public DbSet<api_intiSoft.Models.Clinica.GmSala> GmSala { get; set; } = default!;
        public DbSet<api_intiSoft.Models.Logistica.Producto.ProductoVarianteDetalle> LgProductoVarianteDetalle { get; set; } = default!;
        public DbSet<api_intiSoft.Models.Logistica.Producto.ProductoFichaTecnicaDetalle> LgProductoFichaTecnicaDetalle { get; set; } = default!;
        public DbSet<api_intiSoft.Models.Seguridad.SgUsuario> SgUsuario { get; set; } = default!;
        public DbSet<api_intiSoft.Models.Logistica.Producto.ProductoVariantePresentacion> ProductoVariantePresentacion { get; set; } = default!;
        public DbSet<api_intiSoft.Models.Logistica.Producto.TipoListum> LgTipoListum { get; set; } = default!;
        public DbSet<api_intiSoft.Models.Logistica.Producto.TipoUsoProducto> LgTipoUsoProducto { get; set; } = default!;
        public DbSet<api_intiSoft.Models.Logistica.Producto.ProductoVariantePresentacionDet> LgProductoVariantePresentacionDet { get; set; } = default!;
        
        public DbSet<api_intiSoft.Models.Compras.Proveedor> Proveedor { get; set; } = default!;
        
        
        
        //public DbSet<LgCategorium> LgCategorium { get; set; } = default!;
        //public DbSet<api_intiSoft.Models.Logistica.Producto.LgProducto> LgProducto { get; set; } = default!;


    }
}
