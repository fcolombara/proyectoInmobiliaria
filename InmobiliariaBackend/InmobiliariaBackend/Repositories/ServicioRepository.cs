using InmobiliariaBackend.Data;
using InmobiliariaBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace InmobiliariaBackend.Repositories
{
    public class ServicioRepository : IServicioRepository
    {
        private readonly ApplicationDbContext _context;

        public ServicioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Servicio>> GetAllAsync()
        {
            return await _context.Servicios.Where(s => s.Activo).ToListAsync();
        }

        public async Task<Servicio> GetByIdAsync(int id)
        {
            return await _context.Servicios.FindAsync(id);
        }

        public async Task AddAsync(Servicio servicio)
        {
            await _context.Servicios.AddAsync(servicio);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Servicio servicio)
        {
            _context.Servicios.Update(servicio);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var servicio = await GetByIdAsync(id);
            if (servicio != null)
            {
                servicio.Activo = false; // Borrado lógico
                await _context.SaveChangesAsync();
            }
        }
    }
}