using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplicationAPP.Data;
using WebApplicationAPP.Models;
using WebApplicationAPP.Repositories;
using WebApplicationAPP.ViewModels;

namespace WebApplicationAPP.Controllers
{
    public class AccountController : Controller
    {
        private static readonly string[] RolesPermitidos = [Roles.Administrador, Roles.Cajero];

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUsuarioRepository _usuarioRepository;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUsuarioRepository usuarioRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _usuarioRepository = usuarioRepository;
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new RegisterViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!RolesPermitidos.Contains(model.Rol))
            {
                ModelState.AddModelError(nameof(model.Rol), "Selecciona un rol valido.");
                return View(model);
            }

            var existingUser = await _userManager.FindByEmailAsync(model.Correo);
            if (existingUser is not null)
            {
                ModelState.AddModelError(nameof(model.Correo), "Ese correo ya se encuentra registrado.");
                return View(model);
            }

            Usuario? usuarioSistema = null;

            if (model.Rol == Roles.Cajero)
            {
                usuarioSistema = _usuarioRepository.GetUsuarioByCorreo(model.Correo);

                if (usuarioSistema is null)
                {
                    ModelState.AddModelError(nameof(model.Correo), "El cajero debe existir previamente en el modulo de Usuarios.");
                    return View(model);
                }

                if (!usuarioSistema.Estado)
                {
                    ModelState.AddModelError(nameof(model.Correo), "El usuario del sistema se encuentra inactivo.");
                    return View(model);
                }

                if (usuarioSistema.IdNetUser.HasValue)
                {
                    ModelState.AddModelError(nameof(model.Correo), "Ese cajero ya se encuentra vinculado a una cuenta de acceso.");
                    return View(model);
                }
            }

            var user = new ApplicationUser
            {
                UserName = model.Correo,
                Email = model.Correo,
                NombreCompleto = usuarioSistema is null
                    ? model.Correo
                    : $"{usuarioSistema.Nombres} {usuarioSistema.PrimerApellido} {usuarioSistema.SegundoApellido}".Trim(),
                Carrera = string.Empty,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

            await _userManager.AddToRoleAsync(user, model.Rol);

            if (usuarioSistema is not null)
            {
                usuarioSistema.IdNetUser = Guid.Parse(user.Id);
                _usuarioRepository.UpdateUsuario(usuarioSistema);
            }

            await _signInManager.SignInAsync(user, false);

            TempData["Success"] = "Tu cuenta fue creada correctamente.";
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new LoginViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Correo);

            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "No existe una cuenta registrada con ese correo.");
                return View(model);
            }

            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Any())
            {
                ModelState.AddModelError(string.Empty, "El usuario no tiene permisos asignados en el sistema.");
                return View(model);
            }

            var usuarioSistema = _usuarioRepository.GetUsuarioByIdNetUser(user.Id)
                ?? _usuarioRepository.GetUsuarioByCorreo(model.Correo);

            if (roles.Contains(Roles.Cajero) || roles.Contains(Roles.Contador))
            {
                if (usuarioSistema is null)
                {
                    ModelState.AddModelError(string.Empty, "El cajero no existe en el modulo de Usuarios.");
                    return View(model);
                }

                if (!usuarioSistema.IdNetUser.HasValue)
                {
                    usuarioSistema.IdNetUser = Guid.Parse(user.Id);
                    _usuarioRepository.UpdateUsuario(usuarioSistema);
                }

                if (!usuarioSistema.Estado)
                {
                    ModelState.AddModelError(string.Empty, "El usuario se encuentra inactivo.");
                    return View(model);
                }
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "No fue posible iniciar sesion. Verifica tus credenciales.");
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
