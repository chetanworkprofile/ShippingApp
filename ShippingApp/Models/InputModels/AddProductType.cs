namespace ShippingApp.Models.InputModels
{
    public class AddProductType
    {
        public string type { get; set; } = string.Empty;
        public float price { get; set; } = 0;
        public bool isFragile { get; set; } = true;
    }
}
