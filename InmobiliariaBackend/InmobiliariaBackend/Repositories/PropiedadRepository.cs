using InmobiliariaBackend.Data;
using InmobiliariaBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace InmobiliariaBackend.Repositories
{
	public class PropiedadRepository : IPropiedadRepository
	{
		private readonly ApplicationDbContext _context;

		public PropiedadRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Propiedad>> GetAllAsync()
		{
			return await _context.Propiedades
				.AsNoTracking()
				.Where(p => p.Activo)
				.ToListAsync();
		}

		public async Task<Propiedad?> GetByIdAsync(int id)
		{
			return await _context.Propiedades
				.AsNoTracking()
				.FirstOrDefaultAsync(p => p.Id == id);
		}

		public async Task AddAsync(Propiedad propiedad)
		{
			await _context.Propiedades.AddAsync(propiedad);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateAsync(Propiedad propiedad)
		{
			// --- PASO CLAVE PARA EVITAR EL ERROR DE TRACKING ---
			// 1. Buscamos si el ChangeTracker ya tiene una instancia con ese ID.
			var local = _context.Propiedades.Local.FirstOrDefault(p => p.Id == propiedad.Id);

			// 2. Si existe, la sacamos (Detach) para que no choque con la actualización.
			if (local != null)
			{
				_context.Entry(local).State = EntityState.Detached;
			}

			// 3. Ahora buscamos la entidad fresca de la base de datos.
			var existente = await _context.Propiedades.FindAsync(propiedad.Id);

			if (existente == null)
				throw new Exception("La propiedad no existe en la base de datos.");

			// 4. Mapeamos los valores manualmente (Pisar valores).
			existente.Direccion = propiedad.Direccion;
			existente.Precio = propiedad.Precio;
			existente.Descripcion = propiedad.Descripcion;
			existente.Localidad = propiedad.Localidad;
			existente.TipoOperacion = propiedad.TipoOperacion;
			existente.TipoInmueble = propiedad.TipoInmueble;
			existente.Ambientes = propiedad.Ambientes;
			existente.Color = propiedad.Color;
			existente.Activo = propiedad.Activo;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				throw new Exception("Error de concurrencia: El registro fue modificado por otro usuario.");
			}
		}

		public async Task DeleteAsync(int id)
		{
			var propiedad = await _context.Propiedades.FindAsync(id);
			if (propiedad != null)
			{
				propiedad.Activo = false;
				await _context.SaveChangesAsync();

				// Desvinculamos para evitar problemas en llamadas sucesivas
				_context.Entry(propiedad).State = EntityState.Detached;
			}
		}
	}
}