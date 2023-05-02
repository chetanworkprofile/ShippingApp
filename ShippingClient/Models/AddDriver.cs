using System.ComponentModel.DataAnnotations;

namespace ShippingClient.Models
{
    public class AddDriver
    {
        [Required]
        public string firstName { get; set; }
        [Required]
        public string lastName { get; set; }
        [Required, EmailAddress]
        public string email { get; set; }
        [Required]
        public long contactno { get; set; }
        /*[Required]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Please Enter Valid Password. Must contain atleast one uppercase letter, one lowercase letter, one number and one special chararcter and must be atleast 8 characters long")]
        public string password { get; set; }*/
        public string url { get; set; }
        [Required]
        public string address { get; set; }
        [Required]
        public Guid checkpointLocation { get; set; }
        [Required]
        public bool isAvailable { get; set; }

        //public string pathToProfilePic { get; set; } = string.Empty;

        public AddDriver() { }
    }
}
