using InmobiliariaBackend.Models;
using Microsoft.EntityFrameworkCore;
using InmobiliariaBackend.Data; // <--- Importante

namespace InmobiliariaBackend.Services
{
    public class ServicioService : IServicioService
    {
        private readonly ApplicationDbContext _context; // Reemplaza 'TuDbContext' por el nombre de tu contexto

        public ServicioService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Servicio>> ListarActivos()
        {
            return await _context.Servicios.ToListAsync();
        }

        public async Task Registrar(Servicio servicio)
        {
            _context.Servicios.Add(servicio);
            await _context.SaveChangesAsync();
        }

        // --- MÉTODO ACTUALIZADO ---
        public async Task<bool> Eliminar(int id)
        {
            var tecnico = await _context.Servicios.FindAsync(id);

            if (tecnico == null) return false;

            _context.Servicios.Remove(tecnico);
            await _context.SaveChangesAsync(); // Esto confirma el borrado en MySQL
            return true;
        }
    }
}