using System.ComponentModel.DataAnnotations;

namespace WebApplicationAPP.ViewModels
{
    public class UsuarioCreateViewModel
    {
        [Required(ErrorMessage = "Debe seleccionar un comercio.")]
        [Display(Name = "Comercio")]
        public int IdComercio { get; set; }

        [Required(ErrorMessage = "Los nombres son obligatorios.")]
        [StringLength(100)]
        public string Nombres { get; set; } = string.Empty;

        [Required(ErrorMessage = "El primer apellido es obligatorio.")]
        [StringLength(100)]
        [Display(Name = "Primer Apellido")]
        public string PrimerApellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "El segundo apellido es obligatorio.")]
        [StringLength(100)]
        [Display(Name = "Segundo Apellido")]
        public string SegundoApellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "La identificacion es obligatoria.")]
        [StringLength(10)]
        [Display(Name = "Identificacion")]
        public string Identificacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electronico es obligatorio.")]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo valido.")]
        [StringLength(200)]
        [Display(Name = "Correo Electronico")]
        public string CorreoElectronico { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contrasena es obligatoria.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contrasena")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe confirmar la contrasena.")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "La confirmacion no coincide con la contrasena.")]
        [Display(Name = "Confirmar Contrasena")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
