using Microsoft.AspNetCore.Mvc;
using InmobiliariaBackend.Models;
using InmobiliariaBackend.Services;

namespace InmobiliariaBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly ServicioPedido _service;
        public PedidosController(ServicioPedido service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _service.ListarPedidos());

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PedidoServicio pedido)
        {
            await _service.RegistrarPedido(pedido);
            return Ok(new { message = "Pedido creado" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] PedidoServicio pedido)
        {
            // El técnico solo actualiza el estado usualmente
            await _service.CambiarEstado(id, pedido.Estado);
            return Ok(new { message = "Estado actualizado" });
        }
    }
}