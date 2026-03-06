using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InmobiliariaBackend.Models
{
    public class Propiedad
    {
        [Key]
        public int Id { get; set; }

        public string Direccion { get; set; } = string.Empty;
        public string Precio { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Localidad { get; set; } = string.Empty;
        public string TipoOperacion { get; set; } = "Venta";
        public string TipoInmueble { get; set; } = "Casa";
        public int? Ambientes { get; set; }
        public string Color { get; set; } = "bg-blue-600";
        public bool Activo { get; set; } = true;

        // --- RELACIÓN CON USUARIOS ---

        // Recomendación: usa int? si ya tienes datos en la BD para evitar errores de clave foránea
        [Required]
        public int UsuarioId { get; set; }

        // Propiedad de navegación
        // El [JsonIgnore] es opcional pero útil si empiezas a tener errores de "referencia circular" al traer datos
        [ForeignKey("UsuarioId")]
        public virtual Usuario? Usuario { get; set; }
    }
}