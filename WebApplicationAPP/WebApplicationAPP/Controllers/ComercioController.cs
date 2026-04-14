using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplicationAPP.Bussines;
using WebApplicationAPP.Data;
using WebApplicationAPP.Models;

namespace WebApplicationAPP.Controllers
{
    [Authorize(Roles = Roles.Administrador)]
    public class ComercioController : Controller
    {
        private readonly ComercioBusiness _business;

        public ComercioController(ComercioBusiness business) //Inyección de dependencia del negocio de comercio
        {
            _business = business;
        }

        public IActionResult Index()
        {
            var comercios = _business.GetAll();
            return View(comercios);
        }

        public IActionResult Details(int id)
        {
            var comercio = _business.GetById(id);

            if (comercio == null)
            {
                return NotFound();
            }

            return View(comercio);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //Protección contra ataques CSRF
        public IActionResult Create(Comercio comercio)
        {
            if (!ModelState.IsValid)
            {
                return View(comercio);
            }

            var creado = _business.Add(comercio);

            if (!creado)
            {
                ModelState.AddModelError(string.Empty, "La identificación ya existe."); //Agrega un error de validación si la identificación ya existe
                return View(comercio);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var comercio = _business.GetById(id);

            if (comercio == null)
            {
                return NotFound();
            }

            return View(comercio);
        }

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
