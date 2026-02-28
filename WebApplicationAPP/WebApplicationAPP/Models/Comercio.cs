using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationAPP.Models
{
    [Table("Grupo6_Comercios")]
    public class Comercio
    {
        [Key]
        public int IdComercio { get; set; }

        [Required]
        [StringLength(30)]
        public string Identificacion { get; set; }

        [Required]
        public int TipoIdentificacion { get; set; }

        [Required]
        [StringLength(200)]
        public string Nombre { get; set; }

        [Required]
        public int TipoDeComercio { get; set; }

        [Required]
        [StringLength(20)]
        public string Telefono { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(200)]
        public string CorreoElectronico { get; set; }

        [Required]
        [StringLength(500)]
        public string Direccion { get; set; }

        [Required]
        public DateTime FechaDeRegistro { get; set; }

        public DateTime? FechaDeModificacion { get; set; }

        [Required]
        public bool Estado { get; set; }
    }
}