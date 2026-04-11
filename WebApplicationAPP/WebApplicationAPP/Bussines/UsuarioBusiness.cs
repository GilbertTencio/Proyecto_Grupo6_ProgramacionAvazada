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

        public Usuario? GetById(int id)
        {
            return _usuarioRepository.GetUsuarioById(id);
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
            Usuario? usuario = null;

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var user = new ApplicationUser
                {
                    UserName = model.CorreoElectronico,
                    Email = model.CorreoElectronico,
                    NombreCompleto = $"{model.Nombres} {model.PrimerApellido} {model.SegundoApellido}".Trim(),
                    Carrera = string.Empty
                };

                var identityResult = await _userManager.CreateAsync(user, model.Password);

                if (!identityResult.Succeeded)
                {
                    return (false, string.Join(" ", identityResult.Errors.Select(e => e.Description)));
                }

                usuario = new Usuario
                {
                    IdComercio = model.IdComercio,
                    IdNetUser = Guid.Parse(user.Id),
                    Nombres = model.Nombres,
                    PrimerApellido = model.PrimerApellido,
                    SegundoApellido = model.SegundoApellido,
                    Identificacion = model.Identificacion,
                    CorreoElectronico = model.CorreoElectronico,
                    FechaDeRegistro = DateTime.Now,
                    Estado = true
                };

                _usuarioRepository.AddUsuario(usuario);
                await transaction.CommitAsync();

                _bitacora.RegistrarEvento(
                    "Grupo6_Usuarios",
                    "Registrar",
                    $"Se creo el usuario {usuario.Nombres} {usuario.PrimerApellido}",
                    null,
                    null,
                    usuario
                );

                return (true, "Usuario registrado correctamente.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                _bitacora.RegistrarEvento(
                    "Grupo6_Usuarios",
                    "Error",
                    ex.Message,
                    ex.StackTrace,
                    null,
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

                ApplicationUser? identityUser = null;

                if (existente.IdNetUser.HasValue)
                {
                    identityUser = await _userManager.FindByIdAsync(existente.IdNetUser.Value.ToString());
                }

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

                var usuarioActualizado = new Usuario
                {
                    IdUsuario = model.IdUsuario,
                    IdComercio = existente.IdComercio,
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
                    null,
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
                    ex.StackTrace,
                    null,
                    model
                );

                return (false, "No fue posible actualizar el usuario.");
            }
        }
    }
}
