using InmobiliariaBackend.Models;

namespace InmobiliariaBackend.Services
{
    public interface IPropiedadService
    {
        /// <summary>
        /// Obtiene todas las propiedades (ahora con Direccion y Localidad).
        /// </summary>
        Task<IEnumerable<Propiedad>> ListarTodo();

        /// <summary>
        /// Busca una propiedad específica por su ID único.
        /// </summary>
        Task<Propiedad?> BuscarPorId(int id);

        /// <summary>
        /// Registra una nueva propiedad en la base de datos MySQL.
        /// </summary>
        Task<Propiedad> Crear(Propiedad propiedad);

        /// <summary>
        /// Actualiza los datos de una propiedad existente.
        /// </summary>
        Task Actualizar(Propiedad propiedad);

        /// <summary>
        /// Elimina físicamente el registro de la propiedad por ID.
        /// </summary>
        Task Eliminar(int id);
    }
}