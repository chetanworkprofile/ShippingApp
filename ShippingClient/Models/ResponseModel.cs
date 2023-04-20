namespace ShippingClient.Models
{
    public class ResponseModel
    {
        public int statusCode { get; set; }
        public string message { get; set; } = string.Empty;
        public object data { get; set; } = new object();
        public bool isSuccess { get; set; }
    }
}
