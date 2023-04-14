using System.Diagnostics;

namespace ShippingClient.Models
{
    public class LoginResponse
    {
        public int statusCode{ get; set; }
        public string message { get; set; } = string.Empty; 
        public Data data { get; set; } = new Data();
        public bool isSuccess { get; set; }
    }

    public class Data
    {
        public string userId { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string token { get; set; } = string.Empty;
    }

    public class ErrorLoginResponse
    {
        public int statusCode { get; set; }
        public string message { get; set; } = string.Empty;
        public bool isSuccess { get; set; }
    }
}