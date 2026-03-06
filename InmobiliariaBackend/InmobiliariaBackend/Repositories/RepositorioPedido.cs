using Microsoft.EntityFrameworkCore;
using InmobiliariaBackend.Data;
using InmobiliariaBackend.Models;

namespace InmobiliariaBackend.Repositories
{
    public class RepositorioPedido : IRepositorioPedido
    {
        private readonly ApplicationDbContext _context;

        public RepositorioPedido(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PedidoServicio>> ObtenerTodos()
        {
            return await _context.PedidosServicio.OrderByDescending(p => p.FechaPedido).ToListAsync();
        }

        public async Task<int> Crear(PedidoServicio pedido)
        {
            _context.PedidosServicio.Add(pedido);
            return await _context.SaveChangesAsync(); // EF devuelve la cantidad de filas afectadas
        }

        public async Task<int> ActualizarEstado(int id, string nuevoEstado)
        {
            var pedido = await _context.PedidosServicio.FindAsync(id);
            if (pedido != null)
            {
                pedido.Estado = nuevoEstado;
                return await _context.SaveChangesAsync();
            }
            return 0;
        }
    }
}