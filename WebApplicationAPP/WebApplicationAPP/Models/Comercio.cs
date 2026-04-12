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

        [Required, StringLength(30)]
        public string Identificacion { get; set; } = string.Empty;

        [Required]
        public int TipoIdentificacion { get; set; }

        [Required, StringLength(200)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public int TipoDeComercio { get; set; }

        [Required, StringLength(20)]
        public string Telefono { get; set; } = string.Empty;

        [Required, StringLength(200), EmailAddress]
        public string CorreoElectronico { get; set; } = string.Empty;

        [Required, StringLength(500)]
        public string Direccion { get; set; } = string.Empty;

        [Required]
        public DateTime FechaDeRegistro { get; set; }

        public DateTime? FechaDeModificacion { get; set; }

        [Required]
        public bool Estado { get; set; }

        // 🔥 RELACIONES
        public List<Caja> Cajas { get; set; } = new();
        public ConfiguracionComercio? ConfiguracionComercio { get; set; }
    }
}