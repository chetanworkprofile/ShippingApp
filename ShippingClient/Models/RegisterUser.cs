using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShippingClient.Models
{
    public class RegisterUser
    {
        [Required]
        public string firstName { get; set; }
        [Required]
        public string lastName { get; set; }
        [Required, EmailAddress]
        public string email { get; set; } 
        [Required]
        public long contactno { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string address { get; set; } 
    }
}
