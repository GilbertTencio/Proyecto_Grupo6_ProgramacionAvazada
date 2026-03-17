using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebApplicationAPP.Models;

namespace WebApplicationAPP.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        //Comercio
        public DbSet<Comercio> Comercios { get; set; }

        //Caja
        public DbSet<Caja> Cajas { get; set; }

        //Sinpe
        public DbSet<Sinpe> Sinpes { get; set; }
        public DbSet<BitacoraEvento> BitacoraEventos { get; set; }

        // Ej; clase;
        // public DbSet<Persona> Persona { get; set; }
        // public DbSet<Cliente> Cliente { get; set; }
        // public DbSet<Inventario> Inventario { get; set; }

    }
}
