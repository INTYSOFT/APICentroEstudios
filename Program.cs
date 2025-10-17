using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Finbuckle.MultiTenant;
using api_intiSoft.Data;
using api_intiSoft.Models.Data;
using intiSoft;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using api_intiSoft.Models.Common;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddAutoMapper(typeof(LogisticaProfile));



// Configuración de JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})

.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

//builder.Services.AddAutoMapper(typeof(Program));
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddAuthorization();


// Configuración de la base de datos para el almacenamiento de tenants
builder.Services.AddNpgsql<TenantsDbContext>(builder.Configuration.GetConnectionString("Tenants"));

// Contexto de conexión dinámica sin cadena de conexión explícita
//builder.Services.AddDbContext<ConecDinamicaContext>();

// Inyección de dependencias para los servicios
//builder.Services.AddScoped<IFichaTecnicaService, FichaTecnicaService>();

// Configuración de Finbuckle.MultiTenant
builder.Services
    .AddMultiTenant<CustomTenantInfo>()
    .WithHeaderStrategy("X-Tenant-INTISoft")
    .WithEFCoreStore<TenantsDbContext, CustomTenantInfo>();

// Configuración de controladores y JSON
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPageWeb", policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin()
                     .AllowAnyHeader()
                     .AllowAnyMethod();
    });
});

// Configuración de Swagger
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API intiSoft",
        Version = "v1",
        Description = "API con autenticación JWT"
    });

    // Esquema de seguridad JWT para Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Ingrese el token JWT en este formato: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    // Requerimiento global
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "Bearer",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

var configuration = builder.Configuration;

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<AuditSaveChangesInterceptor>();

builder.Services.AddDbContext<ConecDinamicaContext>((serviceProvider, options) =>
{
    var interceptor = serviceProvider.GetRequiredService<AuditSaveChangesInterceptor>();
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
           .AddInterceptors(interceptor);
});


var app = builder.Build();

app.UseDeveloperExceptionPage(); // durante desarrollo

//  Swagger en producción (por ejemplo, en un entorno interno),
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API intiSoft v1"));
}



app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("CorsPageWeb");
app.UseAuthentication();
app.UseAuthorization();
app.UseMultiTenant();
app.MapControllers();

app.Run();