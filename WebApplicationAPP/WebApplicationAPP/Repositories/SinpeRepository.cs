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

        public List<Sinpe> GetAllSinpes()
        {
            return _context.Sinpes.OrderByDescending(s => s.FechaDeRegistro).ToList();
        }

        public List<Sinpe> GetByTelefonoDestinatario(string telefonoDestino)
        {
            return _context.Sinpes.Where(s => s.TelefonoDestino == telefonoDestino).OrderByDescending(s => s.FechaDeRegistro).ToList();
        }

        public List<Sinpe> GetByComercio(int idCaja)
        {
            return _context.Sinpes.Where(s => s.IdCaja == idCaja).OrderByDescending(s => s.FechaDeRegistro).ToList();
        }

        public void AddSinpe(Sinpe sinpe)
        {
            _context.Sinpes.Add(sinpe);
            _context.SaveChanges();
        }
        public Caja GetCaja(int idCaja)
        {
            return _context.Cajas.FirstOrDefault(c => c.IdCaja == idCaja);
        }
    }
}