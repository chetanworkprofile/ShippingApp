namespace ShippingClient.Models
{
    public class UpdateProductType
    {
        public string productTypeId { get; set; } = string.Empty;
        public string type { get; set; } = string.Empty;
        public float price { get; set; } = 0;
        public bool isFragile { get; set; } = true;
    }
}
