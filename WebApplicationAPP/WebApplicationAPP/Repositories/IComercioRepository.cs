using WebApplicationAPP.Models;

namespace WebApplicationAPP.Repositories
{
    public interface IComercioRepository
    {
        List<Comercio> GetAllComercios();
        Comercio GetComercioById(int id);
        void AddComercio(Comercio comercio);
        void UpdateComercio(Comercio comercio);
        bool ExistsByIdentificacion(string identificacion);

    }
}
