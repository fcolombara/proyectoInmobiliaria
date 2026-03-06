using Microsoft.AspNetCore.Mvc;
using InmobiliariaBackend.Models;
using InmobiliariaBackend.Services;

namespace InmobiliariaBackend.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: api/auth/registrar
        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] Usuario usuario)
        {
            try
            {
                // Extraemos el password del PasswordHash que envía Angular temporalmente
                var nuevoUsuario = await _authService.Registrar(usuario, usuario.PasswordHash);

                return Ok(new
                {
                    message = "Usuario creado con éxito",
                    rol = nuevoUsuario.Rol
                });
            }
            catch (Exception ex)
            {
                // Devolvemos el mensaje de error para que Angular lo muestre en el alert
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var usuario = await _authService.Login(request.Email, request.Password);

            if (usuario == null)
            {
                return Unauthorized("Email o contraseña incorrectos");
            }

            // IMPORTANTE: Estos nombres deben coincidir con lo que usa tu app.ts
            return Ok(new
            {
                id = usuario.Id,
                email = usuario.Email,
                nombre = usuario.NombreCompleto, // Angular espera .nombre
                rol = usuario.Rol               // Angular espera .rol
            });
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}