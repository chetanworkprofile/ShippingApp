namespace ShippingApp.Models.InputModels
{
    public class ResetPasswordModel
    {
        public int otp { get; set; } = 0;
        public string password { get; set; } = string.Empty;
    }
}


// to reset password