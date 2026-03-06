using InmobiliariaBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace InmobiliariaBackend.Repositories
{
    public interface IRepositorioPedido
    {
        Task<IEnumerable<PedidoServicio>> ObtenerTodos();
        Task<int> Crear(PedidoServicio pedido);
        Task<int> ActualizarEstado(int id, string nuevoEstado);
    }
}