using InmobiliariaBackend.Models;
using InmobiliariaBackend.Repositories;

namespace InmobiliariaBackend.Services
{
	public class PropiedadService : IPropiedadService
	{
		private readonly IPropiedadRepository _repository;

		public PropiedadService(IPropiedadRepository repository)
		{
			_repository = repository;
		}

		public async Task<IEnumerable<Propiedad>> ListarTodo()
		{
			return await _repository.GetAllAsync();
		}

		public async Task<Propiedad?> BuscarPorId(int id)
		{
			return await _repository.GetByIdAsync(id);
		}

		public async Task<Propiedad> Crear(Propiedad propiedad)
		{
			// Normalizamos strings para evitar nulos accidentales en la DB
			NormalizarDatos(propiedad);
			ValidarPropiedad(propiedad);

			propiedad.Activo = true;
			await _repository.AddAsync(propiedad);
			return propiedad;
		}

		public async Task Actualizar(Propiedad propiedad)
		{
			if (propiedad.Id <= 0)
				throw new Exception("ID de propiedad inválido para actualizar.");

			// Normalizamos antes de validar y enviar
			NormalizarDatos(propiedad);
			ValidarPropiedad(propiedad);

			// El Repositorio ahora se encarga de la limpieza del ChangeTracker (Detached)
			await _repository.UpdateAsync(propiedad);
		}

		public async Task Eliminar(int id)
		{
			// No validamos existencia aquí para evitar el tracking previo en el DbContext
			await _repository.DeleteAsync(id);
		}

		private void ValidarPropiedad(Propiedad propiedad)
		{
			if (string.IsNullOrWhiteSpace(propiedad.Direccion))
				throw new Exception("La dirección de la propiedad es obligatoria.");

			if (string.IsNullOrWhiteSpace(propiedad.Precio))
				throw new Exception("El precio es obligatorio.");

			if (string.IsNullOrWhiteSpace(propiedad.Localidad))
				throw new Exception("La localidad es obligatoria.");
		}

		// Asegura que los campos opcionales no viajen como null si la DB espera strings
		private void NormalizarDatos(Propiedad propiedad)
		{
			propiedad.Descripcion = propiedad.Descripcion ?? string.Empty;
			propiedad.Color = propiedad.Color ?? "bg-blue-600";
			propiedad.TipoOperacion = propiedad.TipoOperacion ?? "Venta";
			propiedad.TipoInmueble = propiedad.TipoInmueble ?? "Casa";
		}
	}
}