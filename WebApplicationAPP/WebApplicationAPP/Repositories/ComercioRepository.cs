using WebApplicationAPP.Data;
using WebApplicationAPP.Models;

namespace WebApplicationAPP.Repositories
{
    public class ComercioRepository : IComercioRepository
    {
        private readonly AppDbContext _context;

        public ComercioRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Comercio> GetAllComercios()
        {
            return _context.Comercios.ToList();
        }

        public Comercio GetComercioById(int id)
        {
            return _context.Comercios.Find(id);
        }

        public void AddComercio(Comercio comercio)
        {
            _context.Comercios.Add(comercio);
            _context.SaveChanges();
        }

        public void UpdateComercio(Comercio comercio)
        {
            var comercioDb = _context.Comercios.Find(comercio.IdComercio);

            if (comercioDb != null)
            {
                comercioDb.Nombre = comercio.Nombre;
                comercioDb.TipoDeComercio = comercio.TipoDeComercio;
                comercioDb.Telefono = comercio.Telefono;
                comercioDb.CorreoElectronico = comercio.CorreoElectronico;
                comercioDb.Direccion = comercio.Direccion;
                comercioDb.Estado = comercio.Estado;
                comercioDb.FechaDeModificacion = comercio.FechaDeModificacion;

                _context.SaveChanges();
            }
        }

        public bool ExistsByIdentificacion(string identificacion)
        {
            return _context.Comercios.Any(c => c.Identificacion == identificacion);
        }
    }
}