using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationAPP.Models
{
    [Table("Grupo6_BitacoraEventos")]
    public class BitacoraEvento
    {
        [Key]
        public int IdEvento { get; set; }

        public string Tabla { get; set; }

        public string TipoEvento { get; set; }

        public string Descripcion { get; set; }

        public string StackTrace { get; set; }

        public string DatosAnteriores { get; set; }

        public string DatosPosteriores { get; set; }

        public DateTime FechaEvento { get; set; }
    }
}