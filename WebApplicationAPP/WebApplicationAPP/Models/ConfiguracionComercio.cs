using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationAPP.Models
{
    [Table("Grupo6_ConfiguracionComercio")]
    public class ConfiguracionComercio
    {
        [Key]
        public int IdConfiguracion { get; set; }
        public int IdComercio { get; set; }
        public int TipoConfiguracion { get; set; }
        public int Comision { get; set; }
        public DateTime FechaDeRegistro { get; set; }
        public DateTime? FechaDeModificacion { get; set; }
        public bool Estado { get; set; }

        [ForeignKey("IdComercio")]
        public Comercio Comercio { get; set; }
    }
}
