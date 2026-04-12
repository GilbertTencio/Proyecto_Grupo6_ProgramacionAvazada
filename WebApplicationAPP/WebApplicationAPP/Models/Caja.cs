using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationAPP.Models
{
    [Table("Grupo6_Cajas")]
    public class Caja
    {
        [Key]
        public int IdCaja { get; set; }

        [Required]
        public int IdComercio { get; set; }

        [Required, StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required, StringLength(20)]
        public string Telefono { get; set; } = string.Empty;

        [Required]
        public bool Estado { get; set; }

        public DateTime FechaDeRegistro { get; set; }
        public DateTime? FechaDeModificacion { get; set; }

        // 🔥 CAMBIO CLAVE → LISTA
        public List<Sinpe> Sinpes { get; set; } = new();

        [ForeignKey("IdComercio")]
        public Comercio Comercio { get; set; } = null!;

 

    }
}