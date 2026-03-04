using WebApplicationAPP.Data;
using WebApplicationAPP.Models;

namespace WebApplicationAPP.Repositories
{
    public class ComercioRepository : IComercioRepository
    {
        
        private readonly AppDbContext _context;
        public ComercioRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Comercio> GetAllComercios() { 
            return _context.Comercios.ToList();
        }
        public Comercio GetComercioById(int id) { 
            return _context.Comercios.Find(id);
        }

        public void AddComercio(Comercio comercio) { 
            _context.Comercios.Add(comercio);
            _context.SaveChanges();
        }

        public void UpdateComercio(Comercio comercio)
        {
            _context.Comercios.Update(comercio);
            _context.SaveChanges();       
        }

        public bool ExistsByIdentificacion(string identificacion) { 
            return _context.Comercios.Any(comercio => comercio.Identificacion == identificacion);
        }

    }
}
