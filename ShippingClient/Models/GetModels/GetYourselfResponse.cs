namespace ShippingClient.Models
{
    public class GetYourselfResponse
    {
        public int statusCode { get; set; }
        public string message { get; set; } = string.Empty;
        public ResponseUser data { get; set; } = new ResponseUser();
        public bool isSuccess { get; set; }
    }
}
