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
       // Clase;
       // public DbSet<Persona> Persona { get; set; }
       // public DbSet<Cliente> Cliente { get; set; }
       // public DbSet<Inventario> Inventario { get; set; }

    }
}
