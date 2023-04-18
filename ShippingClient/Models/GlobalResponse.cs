namespace ShippingClient.Models
{
    public class GlobalResponse
    {
        public int statusCode { get; set; }
        public string message { get; set; } = string.Empty;
        public Object data { get; set; } = new();
        public bool isSuccess { get; set; }
    }
}
