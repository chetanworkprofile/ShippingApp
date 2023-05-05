using System.ComponentModel.DataAnnotations;

namespace ShippingClient.Models
{
    public class AddDriver
    {
        [Required]
        public string firstName { get; set; } = string.Empty;
        [Required]
        public string lastName { get; set; } = string.Empty;
        [Required, EmailAddress] 
        public string email { get; set; } = string.Empty;
        [Required]
        public long contactno { get; set; } = 0;
        /*[Required]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Please Enter Valid Password. Must contain atleast one uppercase letter, one lowercase letter, one number and one special chararcter and must be atleast 8 characters long")]
        public string password { get; set; }*/
        public string url { get; set; } = string.Empty;
        [Required]
        public string address { get; set; } = string.Empty;
        [Required]
        public Guid checkpointLocation { get; set; } = Guid.Empty;
        [Required]
        public bool isAvailable { get; set; } = true;

        //public string pathToProfilePic { get; set; } = string.Empty;

        public AddDriver() { }
    }
}
