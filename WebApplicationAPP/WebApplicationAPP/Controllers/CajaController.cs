using Microsoft.AspNetCore.Mvc;
using WebApplicationAPP.Bussines;

namespace WebApplicationAPP.Controllers
{
    public class CajaController : Controller
    {
        private readonly SinpeBusiness _sinpeBusiness;

        public CajaController(SinpeBusiness sinpeBusiness)
        {
            _sinpeBusiness = sinpeBusiness;
        }

        // VER SINPES POR CAJA
        public IActionResult VerSinpes(int idCaja)
        {
            var lista = _sinpeBusiness.GetByCaja(idCaja);
            return View(lista);
        }
    }
}