using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplicationAPP.Bussines;
using WebApplicationAPP.Data;
using WebApplicationAPP.Models;

namespace WebApplicationAPP.Controllers
{
    [Authorize(Roles = Roles.Administrador)]
    public class SinpeController : Controller
    {
        private readonly SinpeBusiness _business;
        private readonly AppDbContext _context;

        public SinpeController(SinpeBusiness business, AppDbContext context)
        {
            _business = business;
            _context = context;
        }

        public IActionResult Index()
        {
            var lista = _business.GetAll();
            return View(lista);
        }

        public IActionResult Create()
        {
            CargarCajas();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Sinpe sinpe)
        {
            var resultado = _business.Registrar(sinpe);

            if (!resultado)
            {
                CargarCajas();
                ViewBag.Error = "No se pudo registrar el pago.";
                return View(sinpe);
            }

            return RedirectToAction("Index");
        }

        public IActionResult PorCaja(int idCaja)
        {
            var lista = _business.GetByCaja(idCaja);
            return View("Index", lista);
        }

        private void CargarCajas()
        {
            var cajas = _context.Cajas
                .Include(c => c.Comercio)
                .OrderBy(c => c.Comercio.Nombre)
                .ThenBy(c => c.Nombre)
                .ToList();

            ViewBag.Cajas = new SelectList(cajas, nameof(Caja.IdCaja), nameof(Caja.NombreMostrado));
        }
    }
}
