using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationAPP.Models
{
    [Table("Grupo6_SINPE")]
    public class Sinpe
    {
        [Key]
        public int IdSinpe { get; set; }

        public int IdCaja { get; set; }

        public string TelefonoOrigen { get; set; } = string.Empty;
        public string TelefonoDestino { get; set; } = string.Empty;

        public decimal Monto { get; set; }

        public string Descripcion { get; set; } = string.Empty;

        public bool Estado { get; set; }

        public DateTime FechaDeRegistro { get; set; }

        // 🔥 ESTA ES LA CLAVE

        [ForeignKey("IdCaja")]
        public Caja Caja { get; set; }


    }
}