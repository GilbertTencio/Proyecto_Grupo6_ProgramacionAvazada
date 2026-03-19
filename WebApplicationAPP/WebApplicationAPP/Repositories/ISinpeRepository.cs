using WebApplicationAPP.Models;

namespace WebApplicationAPP.Repositories
{
    public interface ISinpeRepository
    {
        List<Sinpe> GetAllSinpes();
        Sinpe GetByTelefonoDestinatario(string telefonoDestino);
        List<Sinpe> GetByComercio(int idCaja);
        void AddSinpe(Sinpe sinpe);
        Caja GetCaja(int idCaja);
    }
}
