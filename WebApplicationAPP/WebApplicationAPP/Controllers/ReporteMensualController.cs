using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplicationAPP.Data;
using WebApplicationAPP.Repositories;

namespace WebApplicationAPP.Controllers
{
    [Authorize(Roles = Roles.Administrador)]
    public class ReporteMensualController : Controller
    {
        private readonly IReporteMensualRepository _repository;

        public ReporteMensualController(IReporteMensualRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            var reporteMensual = _repository.GetAll();
            return View(reporteMensual);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerarReporteMensual()
        {
            await _repository.GenerarReportesMensuales();
            return RedirectToAction("Index");
        }
    }
}
