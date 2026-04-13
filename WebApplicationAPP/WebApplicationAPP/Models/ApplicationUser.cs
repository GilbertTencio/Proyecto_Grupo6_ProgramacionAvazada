using Microsoft.AspNetCore.Identity;

namespace WebApplicationAPP.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? NombreCompleto { get; set; }
        public string? Carrera { get; set; }
    }
}
