using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using api_intiSoft.Data ;
using Finbuckle.MultiTenant;
using intiSoft;
using api_intiSoft.Service;
using api_intiSoft.Models.Data;


var builder = WebApplication.CreateBuilder(args);
// DB Context's
// Connection string for the tenants database maestra, dsde aca extraemos los datos para la conexión dinámica.
builder.Services.AddNpgsql<TenantsDbContext>(
    builder.Configuration.GetConnectionString("Tenants"));
// Connection database, sin cadena de conexión, esta se define dinámicamente desde el OnConfiguring
builder.Services.AddDbContext<ConecDinamicaContext>();

/*//AddDbContext
//builder.Services.AddDbContext<api_Context>((serviceProvider, options) =>
//{    
//    //var connectionString = Array.IndexOf(args, "api_Context") >= 0
//    //   ? args[Array.IndexOf(args, "api_Context") + 1]
//    //   : serviceProvider.GetRequiredService<IMultiTenantContextAccessor<TenantInfo>>()
//    //       .MultiTenantContext?
//    //       .TenantInfo?
//    //       .ConnectionString; 

//    var connectionString = "Data Source=.;Integrated Security=false;Initial Catalog=db_intiSoft;TrustServerCertificate=True;User ID=omar;Password=mx";

//    options.UseSqlServer(connectionString);

//});*/

//FichaTecnicaService
builder.Services.AddScoped<IFichaTecnicaService, FichaTecnicaService>();

builder.Services.AddControllers();


// Configuración de Finbuckle.MultiTenant
builder.Services
    .AddMultiTenant<CustomTenantInfo>()
    .WithHeaderStrategy("X-Tenant-INTISoft")
    //.WithHostStrategy()
    .WithEFCoreStore<TenantsDbContext, CustomTenantInfo>();

/*builder.Services.
//.WithConfigurationStore();
//.WithStaticStrategy("api_Context");

//.WithConfigurationStore() // Usa la configuración de la aplicación para almacenar la información del inquilino
//.WithRouteStrategy(); // Usa la estrategia de ruta para determinar el inquilino actual


//builder.Services.AddDbContext<api_Context>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("api_Context") ?? throw new InvalidOperationException("Connection string 'api_Context' not found.")));

// Add services to the container.
//builder.Services.AddControllers();
*/

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



//services    .AddControllersWithViews()    .AddNewtonsoftJson();
builder.Services.AddControllersWithViews();
//AddNewtonsoftJson
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "CorsPageWeb",
                 builder =>
                 {
                     builder
                     .WithOrigins("*")
                     .AllowAnyHeader()
                     .AllowAnyMethod();
                     //.WithHeaders("Authorization", "idempresa", "Origin,X-Requested-With", "Content-Type", "Accept")
                     //.WithMethods("GET", "HEAD", "PUT", "PATCH", "POST", "DELETE", "OPTIONS");
                 });
});

var app = builder.Build(); 


// add the Finbuckle.MultiTenant middleware
app.UseMultiTenant();
/*
app.MapGet("/", (HttpContext httpContext) =>
{
    var tenantInfo = httpContext.GetMultiTenantContext<TenantInfo>()?.TenantInfo;

    if (tenantInfo is null)
    {
        return Results.BadRequest();
    }

    return Results.Ok(new
    {
        tenantInfo.Identifier,
        tenantInfo.Id
    });

});*/

/*//Este middleware se utiliza para extraer el ID del tenant de cada solicitud HTTP
//y almacenarlo en los Items del HttpContext, donde puede ser accedido por otros
//componentes de la aplicación.
//app.Use(async (context, next) =>
//{
//    var tenantId = context.Request.Headers["X-Tenant-ID"].ToString();
//    // Aquí puedes almacenar el tenantId en algún lugar donde pueda ser accedido en el resto de tu aplicación.
//    // Por ejemplo, puedes agregarlo a los Items del HttpContext:
//    context.Items["TenantId"] = tenantId;
//    await next.Invoke();
//});
*/

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else 
{
    //app.UseHsts();
}


app.UseCors("CorsPageWeb"); //Habilita CORS en la aplicación.
//app.UseMvc(); //middleware de enrutamiento de MVC.

app.UseHttpsRedirection(); //redirección de HTTP a HTTPS.

// add the Finbuckle.MultiTenant middleware
app.UseMultiTenant();

app.UseRouting(); //enrutamiento de la solicitud HTTP.

app.UseAuthorization();//middleware de autorización.

app.MapControllers();


//endpoint
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

/*app.MapGet("/", (HttpContext httpContext) =>
{
    var tenantInfo = httpContext.GetMultiTenantContext<TenantInfo>()?.TenantInfo;

    if (tenantInfo is null)
    {
        return Results.BadRequest();
    }

    return Results.Ok(new
    {
        tenantInfo.Identifier,
        tenantInfo.Id
    });
});

app.MapGet("/api/products", (ConecDinamicaContext context) =>
    context.LgCategorium.ToListAsync());*/

//await SeedTenantData();

app.Run();

/*async Task SeedTenantData()
{
    using var scope = app.Services.CreateScope();
    var store = scope.ServiceProvider.GetRequiredService<IMultiTenantStore<TenantInfo>>();
    var tenants = await store.GetAllAsync();

    if (tenants.Count() > 0)
    {
        return;
    }

    await store.TryAddAsync(new TenantInfo
    {
        Id = Guid.NewGuid().ToString(),
        Identifier = "tenant01",
        Name = "My Dev Tenant 01",
        ConnectionString = "Server=(localdb)\\mssqllocaldb;Database=ApiMultiTenant_Tenant01;Trusted_Connection=True;MultipleActiveResultSets=true"
    });

    await store.TryAddAsync(new TenantInfo
    {
        Id = Guid.NewGuid().ToString(),
        Identifier = "tenant02",
        Name = "My Dev Tenant 2",
        ConnectionString = "Server=(localdb)\\mssqllocaldb;Database=ApiMultiTenant_Tenant02;Trusted_Connection=True;MultipleActiveResultSets=true"
    });
}*/