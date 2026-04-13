using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationAPP.Data;
using WebApplicationAPP.Models;

[Authorize(Roles = Roles.Administrador)]
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
        ViewBag.Comercios = _context.Comercios.ToList();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ConfiguracionComercio model)
    {
        var existe = _context.ConfiguracionComercio.Any(x => x.IdComercio == model.IdComercio);

        if (existe)
        {
            ModelState.AddModelError(string.Empty, "Este comercio ya tiene configuracion.");
            ViewBag.Comercios = _context.Comercios.ToList();
            return View(model);
        }

        model.FechaDeRegistro = DateTime.Now;
        model.Estado = true;

        _context.ConfiguracionComercio.Add(model);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}
