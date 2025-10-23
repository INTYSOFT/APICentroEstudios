using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Finbuckle.MultiTenant;
using api_intiSoft.Data;                 // TenantsDbContext
using api_intiSoft.Models.Data;          // ConecDinamicaContext, CustomTenantInfo
using api_intiSoft.Models.Common;        // AuditSaveChangesInterceptor
using api_intiSoft.Services.AnswerSheets;// IAnswerSheetProcessor, AnswerSheetProcessor
using Microsoft.Extensions.DependencyInjection;
using OpenCvSharp;                       // Para MapType<Mat> en Swagger (opcional, pero seguro si ya lo usas)
// Para AddSwaggerGenNewtonsoftSupport
using Swashbuckle.AspNetCore.Newtonsoft;
using intiSoft;

System.AppContext.SetSwitch("System.Drawing.EnableUnixSupport", true);

var builder = WebApplication.CreateBuilder(args);

// ================= JWT =================
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? throw new InvalidOperationException("Jwt:Key no configurado"));

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey         = new SymmetricSecurityKey(key),
            ValidateIssuer           = true,
            ValidIssuer              = jwtSettings["Issuer"],
            ValidateAudience         = true,
            ValidAudience            = jwtSettings["Audience"],
            ValidateLifetime         = true,
            ClockSkew                = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// ================= DB / MultiTenant =================
builder.Services.AddNpgsql<TenantsDbContext>(builder.Configuration.GetConnectionString("Tenants"));

builder.Services
    .AddMultiTenant<CustomTenantInfo>()
    .WithHeaderStrategy("X-Tenant-INTISoft")
    .WithEFCoreStore<TenantsDbContext, CustomTenantInfo>();

builder.Services.AddDbContext<ConecDinamicaContext>((sp, options) =>
{
    var interceptor = sp.GetRequiredService<AuditSaveChangesInterceptor>();
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
           .AddInterceptors(interceptor);
});

// ================= Servicios propios =================
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AuditSaveChangesInterceptor>();
builder.Services.AddScoped<IAnswerSheetProcessor, AnswerSheetProcessor>();

// ================= Controllers & JSON =================
// Usas Newtonsoft en el proyecto, mantenemos compatibilidad y evitamos ciclos EF
builder.Services.AddControllers()
    .AddNewtonsoftJson(o =>
    {
        o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        // Si prefieres System.Text.Json, puedes alternar:
        // o.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
    });

// ================= CORS =================
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPageWeb", p =>
    {
        p.AllowAnyOrigin()
         .AllowAnyHeader()
         .AllowAnyMethod();
    });
});

// ================= Swagger =================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title       = "API intiSoft",
        Version     = "v1",
        Description = "API con autenticación JWT"
    });

    // Seguridad JWT en Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Ingrese el token en el formato: Bearer {token}",
        Name        = "Authorization",
        In          = ParameterLocation.Header,
        Type        = SecuritySchemeType.Http,
        Scheme      = "Bearer",
        BearerFormat= "JWT"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                Scheme   = "Bearer",
                Name     = "Bearer",
                In       = ParameterLocation.Header
            },
            new List<string>()
        }
    });

    // Anti-500: evita colisiones y tipos no soportados
    c.CustomSchemaIds(t => t.FullName?.Replace("+", "."));
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    c.SupportNonNullableReferenceTypes();
    c.IgnoreObsoleteActions();
    c.IgnoreObsoleteProperties();

    // Si por error se expone alguno de estos tipos en firmas públicas, mapéalos a binario
    c.MapType<System.Drawing.Image>(() => new OpenApiSchema { Type = "string", Format = "binary" });
    c.MapType<System.Drawing.Bitmap>(() => new OpenApiSchema { Type = "string", Format = "binary" });
    c.MapType<Mat>(() => new OpenApiSchema { Type = "string", Format = "binary" });

    // (Opcional) XML comments si activas el archivo XML en propiedades del proyecto
    // var xml = Path.Combine(AppContext.BaseDirectory, "api_intiSoft.xml");
    // if (File.Exists(xml)) c.IncludeXmlComments(xml);


});

// Soporte para Newtonsoft en Swagger (imprescindible si usas AddNewtonsoftJson)
builder.Services.AddSwaggerGenNewtonsoftSupport();

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    // Rama dedicada a Swagger antes de MultiTenant/Auth
    app.MapWhen(ctx => ctx.Request.Path.StartsWithSegments("/swagger"), swaggerApp =>
    {
        swaggerApp.UseSwagger();
        swaggerApp.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "API intiSoft v1");
            c.RoutePrefix = ""; // UI en /swagger
        });
    });
}



// ================= Pipeline =================
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// MultiTenant temprano en el pipeline (recomendación Finbuckle)
app.UseMultiTenant();

// Swagger habilitado en Dev/Staging
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API intiSoft v1");
        c.RoutePrefix = "swagger"; // UI en /swagger
    });
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("CorsPageWeb");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Endpoint de salud para diagnosticar hosting vs swagger
app.MapGet("/healthz", () => Results.Ok(new { ok = true, ts = DateTime.UtcNow }));

app.Run();
