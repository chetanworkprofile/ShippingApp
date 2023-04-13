using System.ComponentModel.DataAnnotations;

namespace ShippingClient.Models
{
    public class LoginDTO
    {
        [Required]
        public string? email { get; set; }
        [Required]
        public string? password { get; set; }
    }
}
