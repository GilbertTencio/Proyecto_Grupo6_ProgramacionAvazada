using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplicationAPP.Bussines;
using WebApplicationAPP.Models;
using WebApplicationAPP.ViewModels;

namespace WebApplicationAPP.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UsuarioBusiness _business;

        public UsuarioController(UsuarioBusiness business)
        {
            _business = business;
        }

        public IActionResult Index()
        {
            return View(_business.GetAll());
        }

        public IActionResult Create()
        {
            CargarComercios();
            return View(new UsuarioCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuarioCreateViewModel model)
        {
            if (_business.ExistsByIdentificacion(model.Identificacion))
            {
                TempData["Error"] = "Ya existe un usuario con esa identificacion.";
                return RedirectToAction(nameof(Create));
            }

            if (!ModelState.IsValid)
            {
                CargarComercios(model.IdComercio);
                return View(model);
            }

            var resultado = await _business.AddAsync(model);

            if (!resultado.Success)
            {
                ModelState.AddModelError(string.Empty, resultado.Message);
                CargarComercios(model.IdComercio);
                return View(model);
            }

            TempData["Success"] = resultado.Message;
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var usuario = _business.GetEditViewModelById(id);

            if (usuario is null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UsuarioEditViewModel model)
        {
            if (_business.ExistsByIdentificacion(model.Identificacion, model.IdUsuario))
            {
                ModelState.AddModelError(nameof(model.Identificacion), "Ya existe un usuario con esa identificacion.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var resultado = await _business.UpdateAsync(model);

            if (!resultado.Success)
            {
                ModelState.AddModelError(string.Empty, resultado.Message);

                if (resultado.Message == "El usuario no existe.")
                {
                    return NotFound();
                }

                return View(model);
            }

            TempData["Success"] = resultado.Message;
            return RedirectToAction(nameof(Index));
        }

        private void CargarComercios(int? idComercioSeleccionado = null)
        {
            ViewBag.Comercios = new SelectList(
                _business.GetComercios(),
                nameof(Comercio.IdComercio),
                nameof(Comercio.Nombre),
                idComercioSeleccionado
            );
        }
    }
}
