namespace ShippingApp.Models
{
    public class SendEmailModel
    {
        public string Email { get; set; } = string.Empty;
        public int otp { get; set; } = 0;

        public SendEmailModel() { }

        public SendEmailModel(string email, int otp)
        {
            Email = email;
            this.otp = otp;
        }
    }
}
