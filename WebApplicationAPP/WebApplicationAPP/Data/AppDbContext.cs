using Microsoft.AspNetCore.Identity;
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
            builder.Entity<ApplicationUser>().ToTable("AspNetUsers_Grupo6");
            builder.Entity<IdentityRole>().ToTable("AspNetRoles_Grupo6");
            builder.Entity<IdentityUserRole<string>>().ToTable("AspNetUserRoles_Grupo6");
            builder.Entity<IdentityUserClaim<string>>().ToTable("AspNetUserClaims_Grupo6");
            builder.Entity<IdentityUserLogin<string>>().ToTable("AspNetUserLogins_Grupo6");
            builder.Entity<IdentityUserToken<string>>().ToTable("AspNetUserTokens_Grupo6");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("AspNetRoleClaims_Grupo6");

            builder.Entity<Usuario>()
                .HasIndex(u => u.Identificacion)
                .IsUnique();

            builder.Entity<Usuario>()
                .HasOne(u => u.Comercio)
                .WithMany()
                .HasForeignKey(u => u.IdComercio)
                .OnDelete(DeleteBehavior.Restrict);
        }
        //clase
        public DbSet<Comercio> Comercios { get; set; }
        public DbSet<Caja> Cajas { get; set; }
        public DbSet<Sinpe> Sinpes { get; set; }
        public DbSet<BitacoraEvento> BitacoraEventos { get; set; }
        public DbSet<ConfiguracionComercio> ConfiguracionComercio { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<ReporteMensual> ReporteMensual { get; set; }
    }
}
