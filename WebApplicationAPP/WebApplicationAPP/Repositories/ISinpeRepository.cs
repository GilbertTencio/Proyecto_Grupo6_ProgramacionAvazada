using WebApplicationAPP.Models;

namespace WebApplicationAPP.Repositories
{
    public interface ISinpeRepository
    {
        void Add(Sinpe sinpe);

        List<Sinpe> GetAll();

        List<Sinpe> GetByCaja(int idCaja);
    }
}