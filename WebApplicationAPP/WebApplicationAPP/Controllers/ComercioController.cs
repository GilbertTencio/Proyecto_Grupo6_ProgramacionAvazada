using Microsoft.AspNetCore.Mvc;
using WebApplicationAPP.Bussines;
using WebApplicationAPP.Models;

namespace WebApplicationAPP.Controllers
{
    public class ComercioController : Controller
    {
        private readonly ComercioBusiness _business;

        public ComercioController(ComercioBusiness business)
        {
            _business = business;
        }

        // LISTA
        public IActionResult Index()
        {
            var comercios = _business.GetAll();
            return View(comercios);
        }

        // DETAILS
        public IActionResult Details(int id)
        {
            var comercio = _business.GetById(id);

            if (comercio == null)
            {
                return NotFound();
            }

            return View(comercio);
        }

        // CREATE GET
        public IActionResult Create()
        {
            return View();
        }

        // CREATE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Comercio comercio)
        {
            if (!ModelState.IsValid)
            {
                return View(comercio);
            }

            var creado = _business.Add(comercio);

            if (!creado)
            {
                ModelState.AddModelError("", "La identificación ya existe.");
                return View(comercio);
            }

            return RedirectToAction(nameof(Index));
        }

        // EDIT GET
        public IActionResult Edit(int id)
        {
            var comercio = _business.GetById(id);

            if (comercio == null)
            {
                return NotFound();
            }

            return View(comercio);
        }

        // EDIT POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Comercio comercio)
        {
            if (!ModelState.IsValid)
            {
                return View(comercio);
            }

            var actualizado = _business.Update(comercio);

            if (!actualizado)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}