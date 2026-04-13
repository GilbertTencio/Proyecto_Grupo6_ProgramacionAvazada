using WebApplicationAPP.Models;

namespace WebApplicationAPP.Repositories
{
    public interface IUsuarioRepository
    {
        List<Usuario> GetAllUsuarios();
        Usuario? GetUsuarioById(int id);
        void AddUsuario(Usuario usuario);
        void UpdateUsuario(Usuario usuario);
        void DeleteUsuario(int idUsuario);
        bool ExistsByIdentificacion(string identificacion);
        bool ExistsByIdentificacion(string identificacion, int idUsuarioExcluir);
        Usuario? GetUsuarioByIdNetUser(string idNetUser);
        Usuario? GetUsuarioByCorreo(string correoElectronico);
    }
}
