using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplicationAPP.Models;
using WebApplicationAPP.Repositories;
using WebApplicationAPP.ViewModels;

namespace WebApplicationAPP.Controllers
{
    public class AccountController : Controller
    {
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

        public IActionResult Register()
        {
            TempData["Info"] = "El registro de usuarios ahora se administra desde el modulo de Usuarios.";
            return RedirectToAction("Create", "Usuario");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            TempData["Info"] = "El registro de usuarios ahora se administra desde el modulo de Usuarios.";
            return RedirectToAction("Create", "Usuario");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
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

            var usuarioSistema = _usuarioRepository.GetUsuarioByIdNetUser(user.Id)
                ?? _usuarioRepository.GetUsuarioByCorreo(model.Correo);

            if (usuarioSistema is not null && !usuarioSistema.IdNetUser.HasValue)
            {
                usuarioSistema.IdNetUser = Guid.Parse(user.Id);
                _usuarioRepository.UpdateUsuario(usuarioSistema);
            }

            if (usuarioSistema is not null && !usuarioSistema.Estado)
            {
                ModelState.AddModelError(string.Empty, "El usuario se encuentra inactivo.");
                return View(model);
            }

            var passwordCorrect = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!passwordCorrect)
            {
                ModelState.AddModelError(string.Empty, "La contrasena ingresada es incorrecta.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user,
                model.Password,
                false,
                false
            );

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "No fue posible iniciar sesion. Intentalo de nuevo.");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
