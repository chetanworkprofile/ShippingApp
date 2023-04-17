using System.ComponentModel.DataAnnotations;

namespace ShippingClient.Models
{
    public class ResetPasswordModel
    {
        public int otp { get; set; } = 0;
        [Required]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Please Enter Valid Password. Must contain atleast one uppercase letter, one lowercase letter, one number and one special chararcter and must be atleast 8 characters long")]
        public string password { get; set; } = string.Empty;
        public ResetPasswordModel()
        {

        }
        public ResetPasswordModel(int otp, string pass)
        {
            this.otp = otp;
            password = pass;
        }
    }
}
