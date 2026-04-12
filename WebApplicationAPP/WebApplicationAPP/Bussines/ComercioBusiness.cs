using WebApplicationAPP.Models;
using WebApplicationAPP.Repositories;
using WebApplicationAPP.Data;

namespace WebApplicationAPP.Bussines
{
    public class ComercioBusiness
    {
        private readonly IComercioRepository _repository;
        private readonly IBitacoraService _bitacora;
        private readonly AppDbContext _context;

        public ComercioBusiness(
            IComercioRepository repository,
            IBitacoraService bitacora,
            AppDbContext context)
        {
            _repository = repository;
            _bitacora = bitacora;
            _context = context;
        }

        public List<Comercio> GetAll()
        {
            return _repository.GetAllComercios();
        }

        public Comercio GetById(int id)
        {
            return _repository.GetComercioById(id);
        }

        public bool Add(Comercio comercio)
        {
            try
            {
                if (_repository.ExistsByIdentificacion(comercio.Identificacion))
                {
                    return false;
                }

                comercio.FechaDeRegistro = DateTime.Now;
                comercio.Estado = true;

                // 🔥 GUARDAR COMERCIO
                _repository.AddComercio(comercio);

                // 🔥 CALCULAR CANTIDAD DE CAJAS (por comercio)
                var cantidadCajas = _context.Cajas
                    .Count(c => c.IdComercio == comercio.IdComercio);

                var numeroCaja = cantidadCajas + 1;

                var nombreCaja = $"Caja{numeroCaja:D2}";

                // 🔥 CREAR CAJA AUTOMÁTICA
                var caja = new Caja
                {
                    IdComercio = comercio.IdComercio,
                    Nombre = nombreCaja,
                    Telefono = comercio.Telefono,
                    Estado = true,
                    FechaDeRegistro = DateTime.Now
                };

                _context.Cajas.Add(caja);
                _context.SaveChanges();

                // ✅ BITÁCORA SEGURA
                _bitacora.RegistrarEvento(
                    "Grupo6_Comercios",
                    "Registrar",
                    $"Se creó el comercio {comercio.Nombre} con {nombreCaja}",
                    null,
                    null,
                    new
                    {
                        comercio.IdComercio,
                        comercio.Nombre,
                        comercio.Identificacion
                    }
                );

                return true;
            }
            catch (Exception ex)
            {
                _bitacora.RegistrarEvento(
                    "Grupo6_Comercios",
                    "Error",
                    ex.Message,
                    ex.StackTrace,
                    null,
                    new
                    {
                        comercio.Nombre,
                        comercio.Identificacion
                    }
                );

                return false;
            }
        }

        public bool Update(Comercio comercio)
        {
            try
            {
                var existente = _repository.GetComercioById(comercio.IdComercio);

                if (existente == null)
                {
                    return false;
                }

                var anterior = existente;

                comercio.FechaDeModificacion = DateTime.Now;

                _repository.UpdateComercio(comercio);

                _bitacora.RegistrarEvento(
                    "Grupo6_Comercios",
                    "Editar",
                    $"Se editó el comercio {comercio.Nombre}",
                    null,
                    new
                    {
                        anterior.IdComercio,
                        anterior.Nombre
                    },
                    new
                    {
                        comercio.IdComercio,
                        comercio.Nombre
                    }
                );

                return true;
            }
            catch (Exception ex)
            {
                _bitacora.RegistrarEvento(
                    "Grupo6_Comercios",
                    "Error",
                    ex.Message,
                    ex.StackTrace,
                    null,
                    null
                );

                return false;
            }
        }
    }
}