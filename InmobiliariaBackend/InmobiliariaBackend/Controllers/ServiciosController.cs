using Microsoft.AspNetCore.Mvc;
using InmobiliariaBackend.Models;
using InmobiliariaBackend.Services;

namespace InmobiliariaBackend.Controllers
{
    [Route("api/servicios")]
    [ApiController]
    public class ServiciosController : ControllerBase
    {
        private readonly IServicioService _servicioService;

        public ServiciosController(IServicioService servicioService)
        {
            _servicioService = servicioService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Servicio>>> GetServicios()
        {
            var servicios = await _servicioService.ListarActivos();
            return Ok(servicios);
        }

        [HttpPost]
        public async Task<IActionResult> PostServicio([FromBody] Servicio servicio)
        {
            if (servicio == null) return BadRequest(new { message = "Datos inválidos." });

            try
            {
                await _servicioService.Registrar(servicio);
                return Ok(new { message = "Trabajador registrado", id = servicio.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al guardar", error = ex.Message });
            }
        }

        // --- NUEVO MÉTODO DE ELIMINACIÓN ---
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServicio(int id)
        {
            var eliminado = await _servicioService.Eliminar(id);

            if (!eliminado)
            {
                return NotFound(new { message = "Técnico no encontrado en la base de datos." });
            }

            return Ok(new { message = "Técnico eliminado correctamente." });
        }
    }
}