using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationAPP.Models
{
    [Table("Grupo6_ReporteMensual")]
    public class ReporteMensual
    {
        [Key]
        public int IdReporte { get; set; }

        [Required]
        public int IdComercio { get; set; }

        [Required]
        public int CantidadDeCajas { get; set; }

        [Required]
        public decimal MontoTotalRecaudado { get; set; }

        [Required]
        public int CantidadDeSINPES { get; set; }

        [Required]
        public decimal MontoTotalComision { get; set; }

        [Required]
        public DateTime FechaDelReporte { get; set; }

        public virtual Comercio Comercio { get; set; }
    }
}