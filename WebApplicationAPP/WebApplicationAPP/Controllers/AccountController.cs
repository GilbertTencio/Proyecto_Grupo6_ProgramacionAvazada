using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using WebApplicationAPP.Models;
using WebApplicationAPP.ViewModels;

namespace WebApplicationAPP.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Correo,
                    Email = model.Correo,
                    NombreCompleto = model.NombreCompleto,
                    Carrera = string.Empty
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Correo);

            if (user is null)
            {
                ModelState.AddModelError("", "No existe una cuenta registrada con ese correo.");
                return View(model);
            }

            var passwordCorrect = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!passwordCorrect)
            {
                ModelState.AddModelError("", "La contrasena ingresada es incorrecta.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user,
                model.Password,
                false,
                false
            );

            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            ModelState.AddModelError("", "No fue posible iniciar sesion. Intentalo de nuevo.");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
