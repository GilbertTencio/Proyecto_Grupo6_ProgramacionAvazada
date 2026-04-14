using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplicationAPP.Bussines;
using WebApplicationAPP.Data;
using WebApplicationAPP.Repositories;

//Autenticacion
namespace WebApplicationAPP.Controllers
{
    [Authorize(Roles = Roles.CajeroAutorizado)]
    public class CajaController : Controller
    {
        private readonly SinpeBusiness _sinpeBusiness;
        private readonly AppDbContext _context;
        private readonly IUsuarioRepository _usuarioRepository;

        public CajaController(
            SinpeBusiness sinpeBusiness,
            AppDbContext context,
            IUsuarioRepository usuarioRepository)
        {
            _sinpeBusiness = sinpeBusiness;
            _context = context;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<IActionResult> Index()
        {
            var usuario = ObtenerUsuarioActual();

            if (usuario is null)
            {
                return Forbid();
            }

            var cajas = await _context.Cajas
                .AsNoTracking()
                .Where(c => c.IdComercio == usuario.IdComercio)
                .Include(c => c.Comercio) // Incluye la información del comercio
                .OrderBy(c => c.Nombre) // Incluye ordenamiento por nombre de caja
                .ToListAsync();

            return View(cajas);
        }

        public IActionResult VerSinpes(int idCaja)
        {
            var usuario = ObtenerUsuarioActual();

            if (usuario is null)
            {
                return Forbid();
            }

            var caja = _context.Cajas
                .FirstOrDefault(c => c.IdCaja == idCaja && c.IdComercio == usuario.IdComercio);

            if (caja is null)
            {
                return Forbid();
            }

            var lista = _sinpeBusiness.GetByCaja(idCaja); // Obtener los sinpes asociados a la caja seleccionada
            return View(lista);
        }

        private Models.Usuario? ObtenerUsuarioActual()
        {
            var idNetUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return _usuarioRepository.GetUsuarioByIdNetUser(idNetUser ?? string.Empty)
                ?? _usuarioRepository.GetUsuarioByCorreo(User.Identity?.Name ?? string.Empty);
        }
    }
}