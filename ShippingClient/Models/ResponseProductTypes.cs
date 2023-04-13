namespace ShippingClient.Models
{
    public class ResponseProductTypes
    {
        public int statusCode { get; set; } = 200;
        public string message { get; set; } = "Ok";
        public List<ProductType> data { get; set; } = new List<ProductType>();
        public bool isSuccess { get; set; } = true;
    }
}
