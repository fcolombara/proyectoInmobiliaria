using InmobiliariaBackend.Models;
using InmobiliariaBackend.Repositories;

namespace InmobiliariaBackend.Services
{
    public class ServicioPedido
    {
        private readonly IRepositorioPedido _repo;
        public ServicioPedido(IRepositorioPedido repo) => _repo = repo;

        public Task<IEnumerable<PedidoServicio>> ListarPedidos() => _repo.ObtenerTodos();
        public Task<int> RegistrarPedido(PedidoServicio p) => _repo.Crear(p);
        public Task<int> CambiarEstado(int id, string estado) => _repo.ActualizarEstado(id, estado);
    }
}