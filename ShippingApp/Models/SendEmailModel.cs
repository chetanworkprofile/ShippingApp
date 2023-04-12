namespace ShippingApp.Models
{
    public class SendEmailModel
    {
        public string email { get; set; } = string.Empty;
        public string  subject { get; set; } = string.Empty;
        public string body { get; set; } = string.Empty;

        public SendEmailModel() { }

        public SendEmailModel(string email, string subject, string body)
        {
            this.email = email;
            this.subject = subject;
            this.body = body;
        }
    }
}
