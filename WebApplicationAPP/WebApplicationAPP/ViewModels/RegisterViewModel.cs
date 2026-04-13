using System.ComponentModel.DataAnnotations;
using WebApplicationAPP.Data;

namespace WebApplicationAPP.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Ingresa un correo valido.")]
        [Display(Name = "Correo")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contrasena es obligatoria.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contrasena debe tener al menos 6 caracteres.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contrasena")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debes confirmar la contrasena.")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Las contrasenas no coinciden.")]
        [Display(Name = "Confirmar contrasena")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debes seleccionar un rol.")]
        [Display(Name = "Rol")]
        public string Rol { get; set; } = Roles.Administrador;
    }
}
