using WebApplicationAPP.Data;
using WebApplicationAPP.Models;
using WebApplicationAPP.Repositories;

namespace WebApplicationAPP.Bussines
{
    public class SinpeBusiness
    {
        private readonly ISinpeRepository _repository;
        private readonly AppDbContext _context;
        private readonly IBitacoraService _bitacora;

        public SinpeBusiness(
            ISinpeRepository repository,
            AppDbContext context,
            IBitacoraService bitacora)
        {
            _repository = repository;
            _context = context;
            _bitacora = bitacora;
        }

        public bool Registrar(Sinpe sinpe)
        {
            try
            {
                var caja = _context.Cajas.Find(sinpe.IdCaja);

                if (caja == null)
                {
                    _bitacora.RegistrarEvento(
                        "Grupo6_SINPE",
                        "Error",
                        "Caja no existe",
                        "",
                        new { },
                        new { sinpe.IdCaja }
                    );
                    return false;
                }

                if (!caja.Estado)
                {
                    _bitacora.RegistrarEvento(
                        "Grupo6_SINPE",
                        "Error",
                        "Caja inactiva",
                        "",
                        new { },
                        new { sinpe.IdCaja }
                    );
                    return false;
                }

                if (sinpe.Monto <= 0)
                {
                    _bitacora.RegistrarEvento(
                        "Grupo6_SINPE",
                        "Error",
                        "Monto inválido",
                        "",
                        new { },
                        new { sinpe.Monto }
                    );
                    return false;
                }

                sinpe.FechaDeRegistro = DateTime.Now;
                sinpe.Estado = false;

                _repository.Add(sinpe);

                // ✅ BITÁCORA SEGURA
                _bitacora.RegistrarEvento(
                    "Grupo6_SINPE",
                    "Registrar",
                    "Pago registrado",
                    "",
                    new { },
                    new
                    {
                        sinpe.IdCaja,
                        sinpe.Monto,
                        sinpe.TelefonoOrigen,
                        sinpe.Descripcion
                    }
                );

                return true;
            }
            catch (Exception ex)
            {
                _bitacora.RegistrarEvento(
                    "Grupo6_SINPE",
                    "Error",
                    ex.Message,
                    ex.StackTrace ?? "",
                    new { },
                    new { }
                );

                return false;
            }
        }

        public List<Sinpe> GetAll()
        {
            return _repository.GetAll();
        }

        public List<Sinpe> GetByCaja(int idCaja)
        {
            return _repository.GetByCaja(idCaja);
        }
    }
}