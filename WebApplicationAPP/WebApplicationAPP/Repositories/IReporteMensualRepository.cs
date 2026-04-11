using WebApplicationAPP.Models;

namespace WebApplicationAPP.Repositories
{
    public interface IReporteMensualRepository
    {
        void GenerarReportesMensuales(ReporteMensual reporteMensual);
        List<ReporteMensual> GetAll();
    }
}