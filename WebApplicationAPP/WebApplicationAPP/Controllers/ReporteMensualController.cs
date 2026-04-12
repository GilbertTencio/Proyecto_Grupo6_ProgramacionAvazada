using Microsoft.AspNetCore.Mvc;
using WebApplicationAPP.Models;
using WebApplicationAPP.Repositories;

namespace WebApplicationAPP.Controllers
{
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
        public async Task<IActionResult> GenerarReporteMensual()
        {
            await _repository.GenerarReportesMensuales();

            return RedirectToAction("Index");
        }
    }
}