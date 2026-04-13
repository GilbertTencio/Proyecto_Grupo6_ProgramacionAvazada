using System.ComponentModel.DataAnnotations;

namespace WebApplicationAPP.ViewModels
{
    public class ApiTokenRequestViewModel
    {
        [Required]
        public int IdComercio { get; set; }
    }
}
