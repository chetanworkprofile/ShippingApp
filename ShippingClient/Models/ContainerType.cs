namespace ShippingClient.Models
{
    public class ContainerType
    {
        public Guid containerTypeId { get; set; }
        public string containerName { get; set; } = string.Empty;
        public decimal price { get; set; } = decimal.MinValue;
    }
}
