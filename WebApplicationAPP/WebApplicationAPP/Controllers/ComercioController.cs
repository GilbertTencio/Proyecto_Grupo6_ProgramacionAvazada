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
        private readonly AppDbContext _context;

        public ComercioController(ComercioBusiness business, AppDbContext context)
        {
            _business = business;
            _context = context;
        }

        public IActionResult Index()
        {
            try
            {
                var comercios = _business.GetAll();
                return View(comercios);
            }
            catch
            {
                return View("Error");
            }
        }

        public IActionResult Details(int id)
        {
            try
            {
                var comercio = _business.GetById(id);

                if (comercio == null)
                    return NotFound();

                return View(comercio);
            }
            catch
            {
                return View("Error");
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Comercio comercio)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(comercio);

                var creado = _business.Add(comercio);

                if (!creado)
                {
                    ModelState.AddModelError("", "La identificación ya existe.");
                    return View(comercio);
                }

                var existeCaja = _context.Cajas
                    .Any(c => c.Telefono == comercio.Telefono);

                if (!existeCaja)
                {
                    var caja = new Caja
                    {
                        IdComercio = comercio.IdComercio,
                        Nombre = "Caja01",
                        Telefono = comercio.Telefono,
                        Estado = true,
                        FechaDeRegistro = DateTime.Now
                    };

                    _context.Cajas.Add(caja);
                    _context.SaveChanges();
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al crear el comercio: " + ex.Message);
                return View(comercio);
            }
        }

        public IActionResult Edit(int id)
        {
            try
            {
                var comercio = _business.GetById(id);

                if (comercio == null)
                    return NotFound();

                return View(comercio);
            }
            catch
            {
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Comercio comercio)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(comercio);

                var actualizado = _business.Update(comercio);

                if (!actualizado)
                    return NotFound();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al editar: " + ex.Message);
                return View(comercio);
            }
        }
    }
}