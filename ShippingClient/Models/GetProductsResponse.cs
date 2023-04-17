using System.Diagnostics;

namespace ShippingClient.Models
{
    public class GetProductsResponse
    {
        public int statusCode{ get; set; }
        public string message { get; set; } = string.Empty; 
        public List<ProductType> data { get; set; } = new List<ProductType>();
        public bool isSuccess { get; set; }
    }
}