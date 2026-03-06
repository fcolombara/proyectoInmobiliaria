using InmobiliariaBackend.Data;
using InmobiliariaBackend.Repositories;
using InmobiliariaBackend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar Servicios (CORS) - Mantenemos tu política
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- REGISTRO DE CAPAS ---
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPropiedadRepository, PropiedadRepository>();
builder.Services.AddScoped<IPropiedadService, PropiedadService>();
builder.Services.AddScoped<IServicioRepository, ServicioRepository>();
builder.Services.AddScoped<IServicioService, ServicioService>();
builder.Services.AddScoped<IRepositorioPedido, RepositorioPedido>();
builder.Services.AddScoped<ServicioPedido>();

// 2. Configurar Base de Datos (MySQL)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

// 3. Configurar el Pipeline de HTTP (EL ORDEN ES CLAVE AQUÍ)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// IMPORTANTE: UseCors debe ir DESPUÉS de UseRouting (implícito aquí) 
// y ANTES de UseHttpsRedirection o UseAuthorization
app.UseCors("AllowAngular");

// Si vas a probar con http://localhost:5140, puedes comentar temporalmente Redirection 
// para evitar que el navegador se confunda con los certificados SSL
// app.UseHttpsRedirection(); 

app.UseAuthorization();
app.MapControllers();

app.Run();