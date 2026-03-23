namespace WebApplicationAPP.Repositories
{
    public interface IBitacoraService
    {
        void RegistrarEvento(
            string tabla,
            string tipoEvento,
            string descripcion,
            string stackTrace,
            object datosAnteriores,
            object datosPosteriores
        );
    }
}