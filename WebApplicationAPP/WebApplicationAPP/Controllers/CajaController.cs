using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationAPP.Bussines;
using WebApplicationAPP.Data;

namespace WebApplicationAPP.Controllers
{
    [Authorize(Roles = Roles.CajeroAutorizado)]
    public class CajaController : Controller
    {
        private readonly SinpeBusiness _sinpeBusiness;
        private readonly AppDbContext _context;

        public CajaController(SinpeBusiness sinpeBusiness, AppDbContext context)
        {
            _sinpeBusiness = sinpeBusiness;
            _context = context;
        }

        // 🔥 LISTAR CAJAS (LO QUE TE FALTABA)
        public IActionResult Index()
        {
            var cajas = _context.Cajas
                .Include(c => c.Comercio)
                .ToList();

            return View(cajas);
        }

        // 🔥 VER SINPES POR CAJA
        public IActionResult VerSinpes(int idCaja)
        {
            var usuario = ObtenerUsuarioActual();

            if (usuario is null)
            {
                return Forbid();
            }

            var caja = _context.Cajas.FirstOrDefault(c => c.IdCaja == idCaja && c.IdComercio == usuario.IdComercio);

            if (caja is null)
            {
                return Forbid();
            }

            var lista = _sinpeBusiness.GetByCaja(idCaja);
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
