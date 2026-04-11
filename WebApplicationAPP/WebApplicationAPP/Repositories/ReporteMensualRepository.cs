using Microsoft.EntityFrameworkCore;
using WebApplicationAPP.Data;
using WebApplicationAPP.Models;
using WebApplicationAPP.Repositories;

public class ReporteMensualRepository : IReporteMensualRepository
{
    private readonly AppDbContext _context;

    public ReporteMensualRepository(AppDbContext context)
    {
        _context = context;
    }

    public List<ReporteMensual> GetAll()
    {
        return _context.ReporteMensual
            .Include(r=>r.Comercio)
            .OrderByDescending(r=>r.FechaDelReporte)
            .ToList();
    }

    public void GenerarReportesMensuales(ReporteMensual reporteMensual)
    {
        var inicioMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var finMes = inicioMes.AddMonths(1);
        var comercios = _context.Comercios
            .Include(c=>c.Cajas)
                .ThenInclude(cj=>cj.Sinpes)
            .Include(c => c.TipoConfiguracion)
            .ToListAsync();

        foreach (var comercio in comercios)
        {
            var sinpesDelMes = comercio.Cajas
                .SelectMany(c => c.Sinpes)
                .Where(s => s.Fecha >= inicioMes && s.Fecha < finMes)
                .ToList();

            int cantidadCajas = comercio.Cajas.Count;
            int cantidadSinpes = sinpesDelMes.Count;
            decimal montoTotal = sinpesDelMes.Sum(s => s.Monto);
            decimal porcentaje = comercio.TipoConfiguracion.PorcentajeComision / 100m;
            decimal comision = montoTotal * porcentaje;

            var reporteExist = _context.ReporteMensual
                .FirstOrDefault(r =>
                    r.IdComercio == comercio.IdComercio &&
                    r.FechaDelReporte.Year == inicioMes.Year &&
                    r.FechaDelReporte.Month == inicioMes.Month);

            if (reporteExist != null)
            {
                reporteExist.CantidadDeCajas = cantidadCajas;
                reporteExist.CantidadDeSINPES = cantidadSinpes;
                reporteExist.MontoTotalRecaudado = montoTotal;
                reporteExist.MontoTotalComision = comision;
            }
            else
            {
                _context.ReporteMensual.AddAsync(new ReporteMensual
                {
                    IdComercio = comercio.IdComercio,
                    CantidadDeCajas = cantidadCajas,
                    CantidadDeSINPES = cantidadSinpes,
                    MontoTotalRecaudado = montoTotal,
                    MontoTotalComision = comision,
                    FechaDelReporte = inicioMes
                });
            }
        }

        _context.SaveChanges();
    }
}