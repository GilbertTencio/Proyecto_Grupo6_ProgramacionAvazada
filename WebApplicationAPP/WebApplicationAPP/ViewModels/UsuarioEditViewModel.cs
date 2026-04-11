using System.ComponentModel.DataAnnotations;

namespace WebApplicationAPP.ViewModels
{
    public class UsuarioEditViewModel
    {
        public int IdUsuario { get; set; }

        public int IdComercio { get; set; }

        public Guid? IdNetUser { get; set; }

        public DateTime FechaDeRegistro { get; set; }

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

        [Display(Name = "Estado")]
        public bool Estado { get; set; }
    }
}
