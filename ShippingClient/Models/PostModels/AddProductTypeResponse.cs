namespace ShippingClient.Models
{
    public class AddProductTypeResponse
    {
        public int statusCode { get; set; }
        public string message { get; set; } = string.Empty;
        public ProductType data { get; set; } = new ProductType();
        public bool isSuccess { get; set; }
    }
}
