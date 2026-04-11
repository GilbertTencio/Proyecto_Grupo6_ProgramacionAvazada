using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationAPP.Models
{
    [Table("Grupo6_Usuarios")]
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required]
        [Display(Name = "Comercio")]
        public int IdComercio { get; set; }

        public Guid? IdNetUser { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombres { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Display(Name = "Primer Apellido")]
        public string PrimerApellido { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Display(Name = "Segundo Apellido")]
        public string SegundoApellido { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        [Display(Name = "Identificacion")]
        public string Identificacion { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(200)]
        [Display(Name = "Correo Electronico")]
        public string CorreoElectronico { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Fecha de Registro")]
        public DateTime FechaDeRegistro { get; set; }

        [Display(Name = "Fecha de Modificacion")]
        public DateTime? FechaDeModificacion { get; set; }

        [Required]
        public bool Estado { get; set; }

        [ForeignKey(nameof(IdComercio))]
        public Comercio? Comercio { get; set; }
    }
}
