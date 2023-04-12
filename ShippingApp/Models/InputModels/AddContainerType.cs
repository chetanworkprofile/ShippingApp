namespace ShippingApp.Models.InputModels
{
    public class AddContainerType
    {
        public string containerName { get; set; } = string.Empty;
        public decimal price { get; set; } = decimal.Zero;
    }
}
