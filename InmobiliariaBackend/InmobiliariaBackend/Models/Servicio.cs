namespace InmobiliariaBackend.Models
{
	public class Servicio
	{
		public int Id { get; set; }

		// Usamos string.Empty para evitar nulos y facilitar la validación
		public string NombreCompleto { get; set; } = string.Empty;

		// Cambié Cuit a Cuil para que coincida exactamente con tu objeto de Angular
		public string Cuil { get; set; } = string.Empty;

		public string Oficios { get; set; } = string.Empty;

		public string Whatsapp { get; set; } = string.Empty;

		public bool Activo { get; set; } = true;

		// Opcional: Agregar la fecha si quieres registrar cuándo se unió el trabajador
		public DateTime FechaRegistro { get; set; } = DateTime.Now;
	}
}