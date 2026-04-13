using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationAPP.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string NombreCompleto { get; set; }

        [Required]
        [EmailAddress]
        public string Correo { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
