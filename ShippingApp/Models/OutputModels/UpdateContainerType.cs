namespace ShippingApp.Models.OutputModels
{
    public class UpdateContainerType
    {
        public string containerTypeId { get; set; } = string.Empty;
        public string containerName { get; set; } = string.Empty;
        public int price { get; set; } = 0;    
    }
}
