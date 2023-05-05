namespace ShippingClient.Models
{
    public class GlobalResponse
    {
        public int statusCode { get; set; }
        public string message { get; set; } = string.Empty;
        public Object data { get; set; } = new();
        public bool isSuccess { get; set; }

        public GlobalResponse()
        {

        }
        public GlobalResponse(int statusCode, string message, Object data, bool isSuccess)
        {
            this.statusCode = statusCode;
            this.message = message;
            this.data = data;
            this.isSuccess = isSuccess;
        }
    }
}
