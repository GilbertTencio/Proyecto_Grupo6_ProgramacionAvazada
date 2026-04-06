using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationAPP.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Correo { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
