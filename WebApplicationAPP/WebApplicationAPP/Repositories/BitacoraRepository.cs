using WebApplicationAPP.Data;
using WebApplicationAPP.Models;

namespace WebApplicationAPP.Repositories
{
    public class BitacoraRepository : IBitacoraRepository
    {
        private readonly AppDbContext _context;

        public BitacoraRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(BitacoraEvento evento)
        {
            _context.BitacoraEventos.Add(evento);
            _context.SaveChanges();
        }

        public List<BitacoraEvento> GetAll()
        {
            return _context.BitacoraEventos
                .OrderByDescending(e => e.FechaEvento)
                .ToList();
        }
    }
}