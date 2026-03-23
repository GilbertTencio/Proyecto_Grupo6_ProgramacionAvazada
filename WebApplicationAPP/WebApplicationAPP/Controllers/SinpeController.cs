using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplicationAPP.Bussines;
using WebApplicationAPP.Data;
using WebApplicationAPP.Models;

namespace WebApplicationAPP.Controllers
{
    public class SinpeController : Controller
    {
        private readonly SinpeBusiness _business;
        private readonly AppDbContext _context;

        public SinpeController(SinpeBusiness business, AppDbContext context)
        {
            _business = business;
            _context = context;
        }

        // LISTADO GENERAL
        public IActionResult Index()
        {
            var lista = _business.GetAll();
            return View(lista);
        }

        // FORMULARIO
        public IActionResult Create()
        {
            ViewBag.Cajas = new SelectList(_context.Cajas, "IdCaja", "Nombre");
            return View();
        }

        // REGISTRO
        [HttpPost]
        public IActionResult Create(Sinpe sinpe)
        {
            var resultado = _business.Registrar(sinpe);

            if (!resultado)
            {
                ViewBag.Cajas = new SelectList(_context.Cajas, "IdCaja", "Nombre");
                ViewBag.Error = "No se pudo registrar el pago.";
                return View(sinpe);
            }

            return RedirectToAction("Index");
        }

        // VER SINPES POR CAJA
        public IActionResult PorCaja(int idCaja)
        {
            var lista = _business.GetByCaja(idCaja);
            return View("Index", lista);
        }
    }
}