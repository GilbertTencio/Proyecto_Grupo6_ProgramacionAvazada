using Microsoft.EntityFrameworkCore;
using WebApplicationAPP.Data;
using WebApplicationAPP.Models;

namespace WebApplicationAPP.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Usuario> GetAllUsuarios()
        {
            return [.. _context.Usuarios.Include(u => u.Comercio).OrderBy(u => u.Nombres)];
        }

        public Usuario? GetUsuarioById(int id)
        {
            return _context.Usuarios.Find(id);
        }

        public void AddUsuario(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
        }

        public void UpdateUsuario(Usuario usuario)
        {
            var usuarioDb = _context.Usuarios.Find(usuario.IdUsuario);

            if (usuarioDb is null)
            {
                return;
            }

            usuarioDb.IdComercio = usuario.IdComercio;
            usuarioDb.IdNetUser = usuario.IdNetUser;
            usuarioDb.Nombres = usuario.Nombres;
            usuarioDb.PrimerApellido = usuario.PrimerApellido;
            usuarioDb.SegundoApellido = usuario.SegundoApellido;
            usuarioDb.Identificacion = usuario.Identificacion;
            usuarioDb.CorreoElectronico = usuario.CorreoElectronico;
            usuarioDb.FechaDeModificacion = usuario.FechaDeModificacion;
            usuarioDb.Estado = usuario.Estado;

            _context.SaveChanges();
        }

        public void DeleteUsuario(int idUsuario)
        {
            var usuario = _context.Usuarios.Find(idUsuario);

            if (usuario is null)
            {
                return;
            }

            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();
        }

        public bool ExistsByIdentificacion(string identificacion)
        {
            return _context.Usuarios.Any(u => u.Identificacion == identificacion);
        }

        public bool ExistsByIdentificacion(string identificacion, int idUsuarioExcluir)
        {
            return _context.Usuarios.Any(u =>
                u.Identificacion == identificacion &&
                u.IdUsuario != idUsuarioExcluir
            );
        }

        public Usuario? GetUsuarioByIdNetUser(string idNetUser)
        {
            if (!Guid.TryParse(idNetUser, out var idNetUserGuid))
            {
                return null;
            }

            return _context.Usuarios.FirstOrDefault(u => u.IdNetUser == idNetUserGuid);
        }

        public Usuario? GetUsuarioByCorreo(string correoElectronico)
        {
            return _context.Usuarios.FirstOrDefault(u => u.CorreoElectronico == correoElectronico);
        }
    }
}
