using WebApplicationAPP.Models;
using WebApplicationAPP.Repositories;

namespace WebApplicationAPP.Bussines
{
    public class ComercioBusiness
    {
        private readonly IComercioRepository _repository;
        private readonly IBitacoraService _bitacora;

        public ComercioBusiness(IComercioRepository repository, IBitacoraService bitacora)
        {
            _repository = repository;
            _bitacora = bitacora;
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
                // Validar duplicado
                if (_repository.ExistsByIdentificacion(comercio.Identificacion))
                {
                    return false;
                }

                comercio.FechaDeRegistro = DateTime.Now;
                comercio.Estado = true;

                _repository.AddComercio(comercio);

                // BITÁCORA
                _bitacora.RegistrarEvento(
                    "Grupo6_Comercios",
                    "Registrar",
                    $"Se creó el comercio {comercio.Nombre}",
                    null,
                    null,
                    comercio
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

                // BITÁCORA
                _bitacora.RegistrarEvento(
                    "Grupo6_Comercios",
                    "Editar",
                    $"Se editó el comercio {comercio.Nombre}",
                    null,
                    anterior,
                    comercio
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