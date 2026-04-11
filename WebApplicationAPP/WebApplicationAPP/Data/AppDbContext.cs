using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplicationAPP.Models;

namespace WebApplicationAPP.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Usuario>()
                .HasIndex(u => u.Identificacion)
                .IsUnique();

            builder.Entity<Usuario>()
                .HasOne(u => u.Comercio)
                .WithMany()
                .HasForeignKey(u => u.IdComercio)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<Comercio> Comercios { get; set; }
        public DbSet<Caja> Cajas { get; set; }
        public DbSet<Sinpe> Sinpes { get; set; }
        public DbSet<BitacoraEvento> BitacoraEventos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<ReporteMensual> ReporteMensual { get; set; }
    }
}
