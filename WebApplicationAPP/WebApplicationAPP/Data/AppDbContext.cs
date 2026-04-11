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
        }
        //clase
        public DbSet<Comercio> Comercios { get; set; }
        public DbSet<Caja> Cajas { get; set; }
        public DbSet<Sinpe> Sinpes { get; set; }
        public DbSet<BitacoraEvento> BitacoraEventos { get; set; }
        public DbSet<ConfiguracionComercio> ConfiguracionComercio { get; set; }
    }
}
