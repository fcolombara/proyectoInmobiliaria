using System.ComponentModel.DataAnnotations; // Por si usas [Key]
using System;

namespace InmobiliariaBackend.Models
{
    public class PedidoServicio
    {
        public int Id { get; set; }
        public string Direccion { get; set; } = string.Empty;
        public string Localidad { get; set; } = string.Empty;
        public string OficioRequerido { get; set; } = string.Empty; // Electricidad, Plomería, etc.
        public string DescripcionProblema { get; set; } = string.Empty;
        public string Estado { get; set; } = "Pendiente"; // Pendiente, Asignado, Realizado
        public DateTime FechaPedido { get; set; } = DateTime.Now;

        // Relación opcional para saber de qué propiedad viene
        public int PropiedadId { get; set; }
    }
}