using Microsoft.EntityFrameworkCore;
using WebApplicationAPP.Data;
using WebApplicationAPP.Models;

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
            .Include(r => r.Comercio)
            .OrderByDescending(r => r.FechaDelReporte)
            .ToList();
    }

    public async Task GenerarReportesMensuales()
    {
        var inicioMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var finMes = inicioMes.AddMonths(1);

        var comercios = await _context.Comercios
            .Include(c => c.Cajas)
                .ThenInclude(cj => cj.Sinpes)
            .Include(c => c.ConfiguracionComercio)
            .ToListAsync();

        foreach (var comercio in comercios)
        {
            var sinpesDelMes = comercio.Cajas
                .SelectMany(c => c.Sinpes)
                .Where(s => s.FechaDeRegistro >= inicioMes && s.FechaDeRegistro < finMes)
                .ToList();

            var montoTotal = sinpesDelMes.Sum(s => s.Monto);

            var porcentaje = (comercio.ConfiguracionComercio?.Comision ?? 0) / 100m;
            var comision = montoTotal * porcentaje;

            var reporteExist = await _context.ReporteMensual
                .FirstOrDefaultAsync(r =>
                    r.IdComercio == comercio.IdComercio &&
                    r.FechaDelReporte.Year == inicioMes.Year &&
                    r.FechaDelReporte.Month == inicioMes.Month);

            if (reporteExist != null)
            {
                reporteExist.CantidadDeCajas = comercio.Cajas.Count;
                reporteExist.CantidadDeSINPES = sinpesDelMes.Count;
                reporteExist.MontoTotalRecaudado = montoTotal;
                reporteExist.MontoTotalComision = comision;
            }
            else
            {
                await _context.ReporteMensual.AddAsync(new ReporteMensual
                {
                    IdComercio = comercio.IdComercio,
                    CantidadDeCajas = comercio.Cajas.Count,
                    CantidadDeSINPES = sinpesDelMes.Count,
                    MontoTotalRecaudado = montoTotal,
                    MontoTotalComision = comision,
                    FechaDelReporte = inicioMes
                });
            }
        }

        await _context.SaveChangesAsync();
    }
}