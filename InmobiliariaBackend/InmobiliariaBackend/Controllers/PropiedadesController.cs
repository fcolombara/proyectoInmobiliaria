using InmobiliariaBackend.Models;
using InmobiliariaBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropiedadesController : ControllerBase
    {
        private readonly IPropiedadService _service;

        public PropiedadesController(IPropiedadService service)
        {
            _service = service;
        }

        // GET: api/propiedades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Propiedad>>> Get()
        {
            var propiedades = await _service.ListarTodo();
            return Ok(propiedades);
        }

        // GET: api/propiedades/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Propiedad>> Get(int id)
        {
            var propiedad = await _service.BuscarPorId(id);
            if (propiedad == null) return NotFound();
            return Ok(propiedad);
        }

        // POST: api/propiedades
        [HttpPost]
        public async Task<ActionResult<Propiedad>> Post([FromBody] Propiedad propiedad)
        {
            // Validaciones rápidas en el controlador
            if (string.IsNullOrEmpty(propiedad.Direccion) || string.IsNullOrEmpty(propiedad.Precio))
            {
                return BadRequest("La dirección y el precio son obligatorios.");
            }

            try
            {
                var nueva = await _service.Crear(propiedad);
                return CreatedAtAction(nameof(Get), new { id = nueva.Id }, nueva);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno al crear: {ex.Message}");
            }
        }

        // PUT: api/propiedades/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Propiedad propiedad)
        {
            // 1. Verificación de ID
            if (id != propiedad.Id)
                return BadRequest("El ID de la URL no coincide con el objeto enviado.");

            try
            {
                // 2. Llamada directa al servicio. 
                // El Repositorio se encargará de buscar la entidad trackeada y actualizarla.
                await _service.Actualizar(propiedad);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Logueamos el error para debugging
                Console.WriteLine($"Error en PUT Propiedades: {ex.Message}");

                if (ex.Message.Contains("no existe"))
                    return NotFound(ex.Message);

                return StatusCode(500, $"Error al actualizar: {ex.Message}");
            }
        }

        // DELETE: api/propiedades/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // Eliminamos la comprobación "existe" aquí para no ensuciar el ChangeTracker
                await _service.Eliminar(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound($"Error al eliminar: {ex.Message}");
            }
        }
    }
}