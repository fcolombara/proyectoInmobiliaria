using InmobiliariaBackend.Data;
using InmobiliariaBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace InmobiliariaBackend.Services
{
    public interface IAuthService
    {
        Task<Usuario?> Login(string email, string password);
        Task<Usuario> Registrar(Usuario usuario, string password);
    }

    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> Login(string email, string password)
        {
            // Buscamos al usuario por email
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

            if (usuario == null) return null;

            // IMPORTANTE: Aquí deberías usar una librería como BCrypt para comparar hashes.
            // Por ahora, para tu fase de desarrollo, compararemos texto plano, 
            // pero lo marcaremos para mejorar luego.
            if (usuario.PasswordHash != password) return null;

            return usuario;
        }

        public async Task<Usuario> Registrar(Usuario usuario, string password)
        {
            // Por defecto, cualquier registro nuevo es "Cliente"
            // El "Administrador" se crea manualmente en la DB o por un usuario Admin.
            // El "Tecnico" lo registra el Admin según tu requerimiento.

            usuario.PasswordHash = password; // Aquí también iría el Hasheo
            usuario.FechaRegistro = DateTime.Now;
            usuario.Activo = true;

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return usuario;
        }
    }
}