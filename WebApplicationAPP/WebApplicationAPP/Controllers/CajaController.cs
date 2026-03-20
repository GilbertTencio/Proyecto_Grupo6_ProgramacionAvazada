using Microsoft.AspNetCore.Mvc;
using WebApplicationAPP.Bussines;
using WebApplicationAPP.Models;

namespace WebApplicationAPP.Controllers
{
    public class CajaController : Controller
    {
        private readonly SinpeBusiness _sinpeBusiness;

        public IActionResult VerSinpes(string telefono)
        {
            var sinpes = _sinpeBusiness.GetByTelefonoDestinatario(telefono);
            return View(sinpes);
        }

    }
}