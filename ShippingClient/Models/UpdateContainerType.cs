namespace ShippingClient.Models
{
    public class UpdateContainerType
    {
        public string containerTypeId { get; set; } = string.Empty;
        public string containerName { get; set; } = string.Empty;
        public float price { get; set; } = 0;    
    }
}
