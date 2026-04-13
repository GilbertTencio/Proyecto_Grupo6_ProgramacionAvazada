using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationAPP.Data;
using WebApplicationAPP.Models;

public class ConfiguracionController : Controller
{
    private readonly AppDbContext _context;

    public ConfiguracionController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var data = await _context.ConfiguracionComercio
            .Include(c => c.Comercio)
            .ToListAsync();

        return View(data);
    }

    public IActionResult Create()
    {
        var comercios = _context.Comercios.ToList();
        ViewBag.Comercios = comercios;

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ConfiguracionComercio model)
    {
        // 🔥 VALIDACIÓN CLAVE (NO duplicados)
        var existe = _context.ConfiguracionComercio
            .Any(x => x.IdComercio == model.IdComercio);

        if (existe)
        {
            ModelState.AddModelError("", "Este comercio ya tiene configuración");

            // volver a cargar dropdown
            ViewBag.Comercios = _context.Comercios.ToList();
            return View(model);
        }

        // 🔧 DATOS AUTOMÁTICOS
        model.FechaDeRegistro = DateTime.Now;
        model.Estado = true;

        // guardar
        _context.ConfiguracionComercio.Add(model);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

}