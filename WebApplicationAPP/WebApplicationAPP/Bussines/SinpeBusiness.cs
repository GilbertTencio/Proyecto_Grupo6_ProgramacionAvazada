using WebApplicationAPP.Models;
using WebApplicationAPP.Repositories;

namespace WebApplicationAPP.Bussines
{
    public class SinpeBusiness
    {
        private readonly ISinpeRepository _repository;

        public SinpeBusiness(ISinpeRepository repository)
        {
            _repository = repository;
        }

        public List<Sinpe> GetAll()
        {
            return _repository.GetAllSinpes();
        }

        public Sinpe GetByTelefonoDestinatario(string telefonoDestino)
        {
            return _repository.GetByTelefonoDestinatario(telefonoDestino);
        }

        public List<Sinpe> GetByComercio(int idCaja)
        {
            return _repository.GetByComercio(idCaja);
        }

        public bool Add(Sinpe sinpe)
        {
            var caja = _repository.GetCaja(sinpe.IdCaja);

            if (sinpe.Monto <= 0)
            {
                return false;
            }

            if (!caja.Estado)
            {
                return false;
            }

            if (caja.Telefono != sinpe.TelefonoDestino)
            {
                return false;
            }

            sinpe.FechaDeRegistro = DateTime.Now;
            sinpe.Estado = false; 

            _repository.AddSinpe(sinpe);

            return true;
        }
    }
}