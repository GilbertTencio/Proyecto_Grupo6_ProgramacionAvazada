using WebApplicationAPP.Models;
using WebApplicationAPP.Repositories;

namespace WebApplicationAPP.Bussines
{
    public class ComercioBusiness
    {
        private readonly IComercioRepository _repository;

        public ComercioBusiness(IComercioRepository repository)
        {
            _repository = repository;
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
            // validar duplicado
            if (_repository.ExistsByIdentificacion(comercio.Identificacion))
            {
                return false;
            }

            comercio.FechaDeRegistro = DateTime.Now;
            comercio.Estado = true;

            _repository.AddComercio(comercio);

            return true;
        }

        public bool Update(Comercio comercio)
        {
            var existente = _repository.GetComercioById(comercio.IdComercio);

            if (existente == null)
            {
                return false;
            }

            comercio.FechaDeModificacion = DateTime.Now;

            _repository.UpdateComercio(comercio);

            return true;
        }
    }
}