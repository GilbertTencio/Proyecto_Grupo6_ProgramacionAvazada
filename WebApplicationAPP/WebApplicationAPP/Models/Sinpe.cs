using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationAPP.Models
{
    [Table("Grupo6_SINPE")]
    public class Sinpe
    {
        [Key]
        public int IdSinpe { get; set; }

        [Required]
        public int IdCaja { get; set; }

        [Required]
        public string TelefonoOrigen { get; set; }

        [Required]
        public string TelefonoDestino { get; set; }

        [Required]
        public decimal Monto { get; set; }

        public string Descripcion { get; set; }

        public bool Estado { get; set; }

        public DateTime FechaDeRegistro { get; set; }
    }
}