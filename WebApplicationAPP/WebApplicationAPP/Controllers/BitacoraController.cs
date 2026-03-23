using Microsoft.AspNetCore.Mvc;
using WebApplicationAPP.Data;

namespace WebApplicationAPP.Controllers
{
    public class BitacoraController : Controller
    {
        private readonly AppDbContext _context;

        public BitacoraController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var eventos = _context.BitacoraEventos
                .OrderByDescending(e => e.FechaEvento)
                .ToList();

            return View(eventos);
        }
    }
}