using System.ComponentModel.DataAnnotations;

namespace ShippingClient.Models
{
    public class ChangePasswordModel
    {
        [Required]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Please Enter Valid Old Password. Must contain atleast one uppercase letter, one lowercase letter, one number and one special chararcter and must be atleast 8 characters long")]
        public string oldPassword { get; set; } = string.Empty;
        [Required]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Please Enter Valid New Password. Must contain atleast one uppercase letter, one lowercase letter, one number and one special chararcter and must be atleast 8 characters long")]
        public string newPassword { get; set; } = string.Empty;
        public ChangePasswordModel()
        {

        }
        public ChangePasswordModel(string oldPassword, string newPassword)
        {
            this.oldPassword = oldPassword;
            this.newPassword = newPassword;
        }
    }
}
