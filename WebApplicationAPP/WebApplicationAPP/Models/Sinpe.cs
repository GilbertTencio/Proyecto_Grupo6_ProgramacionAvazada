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

        public int IdCaja { get; set; }

        public string TelefonoOrigen { get; set; } = string.Empty;

        public decimal Monto { get; set; }

        public string Descripcion { get; set; } = string.Empty;

        public DateTime FechaDeRegistro { get; set; }

        public bool Estado { get; set; }
    }
}
