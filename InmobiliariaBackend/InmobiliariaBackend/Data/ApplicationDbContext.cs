using Microsoft.EntityFrameworkCore;
using InmobiliariaBackend.Models;

namespace InmobiliariaBackend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Propiedad> Propiedades { get; set; }
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<PedidoServicio> PedidosServicio { get; set; }
        // NUEVO: DbSet para los usuarios
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // CONFIGURACIÓN: Usuarios (NUEVO)
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuarios");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).HasColumnName("email").IsRequired();
                entity.Property(e => e.PasswordHash).HasColumnName("password_hash").IsRequired();
                entity.Property(e => e.NombreCompleto).HasColumnName("nombre_completo");
                entity.Property(e => e.Rol).HasColumnName("rol").IsRequired();
                entity.Property(e => e.FechaRegistro).HasColumnName("fecha_registro");
                entity.Property(e => e.Activo).HasColumnName("activo");
            });

            // CONFIGURACIÓN: Propiedades (ACTUALIZADO con relación)
            modelBuilder.Entity<Propiedad>(entity =>
            {
                entity.ToTable("propiedades");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TipoOperacion).HasColumnName("tipo_operacion");
                entity.Property(e => e.TipoInmueble).HasColumnName("tipo_inmueble");

                // Mapeo de la nueva relación
                entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

                // Configuración de la relación 1 a Muchos (Un usuario tiene muchas propiedades)
                entity.HasOne(p => p.Usuario)
                      .WithMany()
                      .HasForeignKey(p => p.UsuarioId)
                      .OnDelete(DeleteBehavior.Restrict); // Evita borrar un usuario si tiene propiedades cargadas
            });

            // CONFIGURACIÓN: Servicios
            modelBuilder.Entity<Servicio>(entity =>
            {
                entity.ToTable("servicios");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NombreCompleto).HasColumnName("nombre_completo");
                entity.Property(e => e.Cuil).HasColumnName("cuil");
                entity.Property(e => e.Oficios).HasColumnName("oficios");
                entity.Property(e => e.Whatsapp).HasColumnName("whatsapp");
                entity.Property(e => e.Activo).HasColumnName("activo");
            });

            // CONFIGURACIÓN: Pedidos de Servicio
            modelBuilder.Entity<PedidoServicio>(entity =>
            {
                entity.ToTable("pedidos_servicio");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Direccion).HasColumnName("direccion");
                entity.Property(e => e.Localidad).HasColumnName("localidad");
                entity.Property(e => e.OficioRequerido).HasColumnName("oficio_requerido");
                entity.Property(e => e.DescripcionProblema).HasColumnName("descripcion_problema");
                entity.Property(e => e.Estado).HasColumnName("estado");
                entity.Property(e => e.FechaPedido).HasColumnName("fecha_pedido");
                entity.Property(e => e.PropiedadId).HasColumnName("propiedad_id");
            });
        }
    }
}