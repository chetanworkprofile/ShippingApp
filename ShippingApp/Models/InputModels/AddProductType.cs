namespace ShippingApp.Models.InputModels
{
    public class AddProductType
    {
        public string type { get; set; } = string.Empty;
        public decimal price { get; set; } = decimal.MinValue;
        public bool isFragile { get; set; } = true;
    }
}
