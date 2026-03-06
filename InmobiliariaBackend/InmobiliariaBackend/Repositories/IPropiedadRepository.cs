using InmobiliariaBackend.Models;

namespace InmobiliariaBackend.Repositories
{
    public interface IPropiedadRepository
    {
        // Obtiene todas las propiedades mapeando las nuevas columnas de MySQL
        Task<IEnumerable<Propiedad>> GetAllAsync();

        // Obtiene una propiedad por ID (mapea direccion y localidad)
        Task<Propiedad?> GetByIdAsync(int id);

        // Inserta un registro en la tabla 'propiedades' usando los nuevos nombres de columna
        Task AddAsync(Propiedad propiedad);

        // Actualiza el registro asegurando que 'direccion' y 'localidad' se guarden correctamente
        Task UpdateAsync(Propiedad propiedad);

        // Elimina el registro por ID
        Task DeleteAsync(int id);
    }
}
