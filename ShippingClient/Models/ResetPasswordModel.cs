namespace ShippingClient.Models
{
    public class ResetPasswordModel
    {
        public int otp { get; set; } = 0;
        public string password { get; set; } = string.Empty;
        public ResetPasswordModel(int otp, string pass)
        {
            this.otp = otp;
            password = pass;
        }
    }
}
