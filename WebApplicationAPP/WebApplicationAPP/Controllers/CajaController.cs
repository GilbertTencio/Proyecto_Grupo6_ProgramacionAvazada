using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationAPP.Bussines;
using WebApplicationAPP.Data;

namespace WebApplicationAPP.Controllers
{
    public class CajaController : Controller
    {
        private readonly SinpeBusiness _sinpeBusiness;
        private readonly AppDbContext _context;

        public CajaController(SinpeBusiness sinpeBusiness, AppDbContext context)
        {
            _sinpeBusiness = sinpeBusiness;
            _context = context;
        }

        // 🔥 LISTAR CAJAS (LO QUE TE FALTABA)
        public IActionResult Index()
        {
            var cajas = _context.Cajas
                .Include(c => c.Comercio)
                .ToList();

            return View(cajas);
        }

        // 🔥 VER SINPES POR CAJA
        public IActionResult VerSinpes(int idCaja)
        {
            var lista = _sinpeBusiness.GetByCaja(idCaja);
            return View(lista);
        }
    }
}