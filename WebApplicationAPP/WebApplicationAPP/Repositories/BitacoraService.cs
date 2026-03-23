using System.Text.Json;
using WebApplicationAPP.Data;
using WebApplicationAPP.Models;

namespace WebApplicationAPP.Repositories
{
    public class BitacoraService : IBitacoraService
    {
        private readonly AppDbContext _context;

        public BitacoraService(AppDbContext context)
        {
            _context = context;
        }

        public void RegistrarEvento(
            string tabla,
            string tipoEvento,
            string descripcion,
            string stackTrace,
            object datosAnteriores,
            object datosPosteriores)
        {
            var evento = new BitacoraEvento
            {
                Tabla = tabla,
                TipoEvento = tipoEvento,
                FechaEvento = DateTime.Now,
                Descripcion = descripcion,
                StackTrace = stackTrace,
                DatosAnteriores = JsonSerializer.Serialize(datosAnteriores),
                DatosPosteriores = JsonSerializer.Serialize(datosPosteriores)
            };

            _context.BitacoraEventos.Add(evento);
            _context.SaveChanges();
        }
    }
}