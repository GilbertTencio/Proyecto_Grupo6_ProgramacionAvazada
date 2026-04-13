using Microsoft.AspNetCore.Identity;
using WebApplicationAPP.Data;
using WebApplicationAPP.Models;
using WebApplicationAPP.Repositories;
using WebApplicationAPP.ViewModels;

namespace WebApplicationAPP.Bussines
{
    public class UsuarioBusiness
    {
        private readonly AppDbContext _context;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IComercioRepository _comercioRepository;
        private readonly IBitacoraService _bitacora;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsuarioBusiness(
            AppDbContext context,
            IUsuarioRepository usuarioRepository,
            IComercioRepository comercioRepository,
            IBitacoraService bitacora,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _usuarioRepository = usuarioRepository;
            _comercioRepository = comercioRepository;
            _bitacora = bitacora;
            _userManager = userManager;
        }

        public List<Usuario> GetAll()
        {
            return _usuarioRepository.GetAllUsuarios();
        }

        public UsuarioEditViewModel? GetEditViewModelById(int id)
        {
            var usuario = _usuarioRepository.GetUsuarioById(id);

            if (usuario is null)
            {
                return null;
            }

            return new UsuarioEditViewModel
            {
                IdUsuario = usuario.IdUsuario,
                IdComercio = usuario.IdComercio,
                IdNetUser = usuario.IdNetUser,
                FechaDeRegistro = usuario.FechaDeRegistro,
                Nombres = usuario.Nombres,
                PrimerApellido = usuario.PrimerApellido,
                SegundoApellido = usuario.SegundoApellido,
                Identificacion = usuario.Identificacion,
                CorreoElectronico = usuario.CorreoElectronico,
                Estado = usuario.Estado
            };
        }

        public List<Comercio> GetComercios()
        {
            return _comercioRepository.GetAllComercios();
        }

        public bool ExistsByIdentificacion(string identificacion)
        {
            return _usuarioRepository.ExistsByIdentificacion(identificacion);
        }

        public bool ExistsByIdentificacion(string identificacion, int idUsuarioExcluir)
        {
            return _usuarioRepository.ExistsByIdentificacion(identificacion, idUsuarioExcluir);
        }

        public async Task<(bool Success, string Message)> AddAsync(UsuarioCreateViewModel model)
        {
            var usuario = new Usuario
            {
                IdComercio = model.IdComercio,
                Nombres = model.Nombres,
                PrimerApellido = model.PrimerApellido,
                SegundoApellido = model.SegundoApellido,
                Identificacion = model.Identificacion,
                CorreoElectronico = model.CorreoElectronico,
                FechaDeRegistro = DateTime.Now,
                Estado = true
            };

            try
            {
                _usuarioRepository.AddUsuario(usuario);

                _bitacora.RegistrarEvento(
                    "Grupo6_Usuarios",
                    "Registrar",
                    $"Se creo el usuario {usuario.Nombres} {usuario.PrimerApellido}",
                    string.Empty,
                    new { },
                    usuario
                );

                return (true, "Usuario registrado correctamente.");
            }
            catch (Exception ex)
            {
                _bitacora.RegistrarEvento(
                    "Grupo6_Usuarios",
                    "Error",
                    ex.Message,
                    ex.StackTrace ?? string.Empty,
                    new { },
                    model
                );

                return (false, "No fue posible registrar el usuario.");
            }
        }

        public async Task<(bool Success, string Message)> UpdateAsync(UsuarioEditViewModel model)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var existente = _usuarioRepository.GetUsuarioById(model.IdUsuario);

                if (existente is null)
                {
                    return (false, "El usuario no existe.");
                }

                if (existente.IdNetUser.HasValue)
                {
                    var identityUser = await _userManager.FindByIdAsync(existente.IdNetUser.Value.ToString());

                    if (identityUser is not null)
                    {
                        identityUser.Email = model.CorreoElectronico;
                        identityUser.UserName = model.CorreoElectronico;
                        identityUser.NombreCompleto = $"{model.Nombres} {model.PrimerApellido} {model.SegundoApellido}".Trim();

                        var identityResult = await _userManager.UpdateAsync(identityUser);

                        if (!identityResult.Succeeded)
                        {
                            return (false, string.Join(" ", identityResult.Errors.Select(e => e.Description)));
                        }
                    }
                }

                var usuarioActualizado = new Usuario
                {
                    IdUsuario = model.IdUsuario,
                    IdComercio = model.IdComercio,
                    IdNetUser = existente.IdNetUser,
                    Nombres = model.Nombres,
                    PrimerApellido = model.PrimerApellido,
                    SegundoApellido = model.SegundoApellido,
                    Identificacion = model.Identificacion,
                    CorreoElectronico = model.CorreoElectronico,
                    FechaDeRegistro = existente.FechaDeRegistro,
                    FechaDeModificacion = DateTime.Now,
                    Estado = model.Estado
                };

                _usuarioRepository.UpdateUsuario(usuarioActualizado);
                await transaction.CommitAsync();

                _bitacora.RegistrarEvento(
                    "Grupo6_Usuarios",
                    "Editar",
                    $"Se edito el usuario {usuarioActualizado.Nombres} {usuarioActualizado.PrimerApellido}",
                    string.Empty,
                    existente,
                    usuarioActualizado
                );

                return (true, "Usuario actualizado correctamente.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                _bitacora.RegistrarEvento(
                    "Grupo6_Usuarios",
                    "Error",
                    ex.Message,
                    ex.StackTrace ?? string.Empty,
                    new { },
                    model
                );

                return (false, "No fue posible actualizar el usuario.");
            }
        }

        public async Task<(bool Success, string Message)> DeleteAsync(int idUsuario)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var usuario = _usuarioRepository.GetUsuarioById(idUsuario);

                if (usuario is null)
                {
                    return (false, "El usuario no existe.");
                }

                if (usuario.IdNetUser.HasValue)
                {
                    var identityUser = await _userManager.FindByIdAsync(usuario.IdNetUser.Value.ToString());

                    if (identityUser is not null)
                    {
                        var result = await _userManager.DeleteAsync(identityUser);

                        if (!result.Succeeded)
                        {
                            return (false, string.Join(" ", result.Errors.Select(e => e.Description)));
                        }
                    }
                }

                _usuarioRepository.DeleteUsuario(idUsuario);
                await transaction.CommitAsync();

                _bitacora.RegistrarEvento(
                    "Grupo6_Usuarios",
                    "Eliminar",
                    $"Se elimino el usuario {usuario.Nombres} {usuario.PrimerApellido}",
                    string.Empty,
                    usuario,
                    new { }
                );

                return (true, "Usuario eliminado correctamente.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                _bitacora.RegistrarEvento(
                    "Grupo6_Usuarios",
                    "Error",
                    ex.Message,
                    ex.StackTrace ?? string.Empty,
                    new { IdUsuario = idUsuario },
                    new { }
                );

                return (false, "No fue posible eliminar el usuario.");
            }
        }
    }
}
