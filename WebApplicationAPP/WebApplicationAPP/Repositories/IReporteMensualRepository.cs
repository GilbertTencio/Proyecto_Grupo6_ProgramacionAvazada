using WebApplicationAPP.Models;
public interface IReporteMensualRepository
{
    List<ReporteMensual> GetAll();
    Task GenerarReportesMensuales();
}