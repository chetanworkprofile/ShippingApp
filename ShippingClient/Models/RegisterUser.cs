using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ShippingClient.Models
{
    public class RegisterUser
    {
        [Required]
        public string firstName { get; set; } = string.Empty;
        [Required]
        public string lastName { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string email { get; set; } = string.Empty;
        [Required]
        //[RegularExpression(@"^[6-9]\\d{9}$", ErrorMessage = "Please Enter Valid PhoneNo")]
        public long contactno { get; set; } = 9999999999;
        [Required]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Please Enter Valid Password. Must contain atleast one uppercase letter, one lowercase letter, one number and one special chararcter and must be atleast 8 characters long")]

        public string password { get; set; } = string.Empty;
        [Required]
        public string address { get; set; } = string.Empty; 
    }
}
