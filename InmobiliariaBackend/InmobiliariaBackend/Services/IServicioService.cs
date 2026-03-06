using InmobiliariaBackend.Models;

public interface IServicioService
{
	Task<IEnumerable<Servicio>> ListarActivos();
	Task Registrar(Servicio servicio);
    Task<bool> Eliminar(int id);
}