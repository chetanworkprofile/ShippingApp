namespace ShippingClient.Models
{
    public class LoginResponse
    {
        public int statusCode{ get; set; }
        public string message { get; set; }
        public Data data { get; set; }
        public bool isSuccess { get; set; }
    }

    public class Data
    {
        public string userId { get; set; }
        public string email { get; set; }
        public string token { get; set; }
    }

    public class ErrorLoginResponse
    {
        public int statusCode { get; set; }
        public string message { get; set; }
        public bool isSuccess { get; set; }
    }
}