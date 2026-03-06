using InmobiliariaBackend.Data;
using InmobiliariaBackend.Models; // Añadido para reconocer la clase Usuario
using InmobiliariaBackend.Repositories;
using InmobiliariaBackend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar Servicios (CORS)
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

// 2. Configurar Base de Datos (SQL Server)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=inmobiliaria.db"));

var app = builder.Build();

// --- BLOQUE DE EMERGENCIA: CREAR BASE DE DATOS Y ADMIN ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();

        // Crea la base de datos y las tablas si no existen (sin usar migraciones)
        context.Database.EnsureCreated();

        // Si la tabla de usuarios está vacía, creamos el primer Admin
        if (!context.Usuarios.Any())
        {
            context.Usuarios.Add(new Usuario
            {
                NombreCompleto = "Admin Inicial",
                Email = "admin@inmo.com",
                PasswordHash = "admin123", // Texto plano por ahora para pruebas
                Rol = "Administrador",
                Activo = true,
                FechaRegistro = DateTime.Now
            });
            context.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error al inicializar la base de datos.");
    }
}
// --------------------------------------------------------

// 3. Configurar el Pipeline de HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngular");

app.UseAuthorization();
app.MapControllers();

app.Run();