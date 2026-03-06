using System.ComponentModel.DataAnnotations;

namespace InmobiliariaBackend.Models
{
	public class Usuario
	{
		public int Id { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; } = string.Empty;

		[Required]
		public string PasswordHash { get; set; } = string.Empty;

		[Required]
		public string NombreCompleto { get; set; } = string.Empty;

		// Aquí definimos el Rol: "Administrador", "Cliente" o "Tecnico"
		[Required]
		public string Rol { get; set; } = "Cliente";

		public DateTime FechaRegistro { get; set; } = DateTime.Now;

		public bool Activo { get; set; } = true;
	}
}