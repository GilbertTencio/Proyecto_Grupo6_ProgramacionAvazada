using Microsoft.EntityFrameworkCore;
using WebApplicationAPP.Data;
using WebApplicationAPP.Models;

namespace WebApplicationAPP.Repositories
{
    public class SinpeRepository : ISinpeRepository
    {
        private readonly AppDbContext _context;

        public SinpeRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Sinpe sinpe)
        {
            _context.Sinpes.Add(sinpe);
            _context.SaveChanges();
        }

        public List<Sinpe> GetAll()
        {
            return _context.Sinpes
                .Include(s => s.Caja)
                .ThenInclude(c => c.Comercio)
                .OrderByDescending(s => s.FechaDeRegistro)
                .ToList();
        }

        public List<Sinpe> GetByCaja(int idCaja)
        {
            return _context.Sinpes
                .Include(s => s.Caja)
                .ThenInclude(c => c.Comercio)
                .Where(s => s.IdCaja == idCaja)
                .OrderByDescending(s => s.FechaDeRegistro)
                .ToList();
        }
    }
}
