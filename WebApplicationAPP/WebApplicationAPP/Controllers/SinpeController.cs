using Microsoft.AspNetCore.Mvc;
using WebApplicationAPP.Bussines;
using WebApplicationAPP.Models;

namespace WebApplicationAPP.Controllers
{
    public class SinpeController : Controller
    {
        private readonly SinpeBusiness _business;

        public SinpeController(SinpeBusiness business)
        {
            _business = business;
        }
        public IActionResult Index()
        {
            var sinpes = _business.GetAll();
            return View(sinpes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Sinpe sinpe)
        {
            if (!ModelState.IsValid)
            {
                return View(sinpe);
            }

            var creado = _business.Add(sinpe);

            if (!creado)
            {
                ModelState.AddModelError("", "Verifique que el teléfono de destino este registrado, que la caja se encuentre activa y que el valor del monto sea mayor a 0.");
                return View(sinpe);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}