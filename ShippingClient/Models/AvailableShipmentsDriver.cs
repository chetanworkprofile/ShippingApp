namespace ShippingClient.Models
{
    public class AvailableShipmentsDriver
    {
        public Guid mapId { get; set; }
        public Guid shipmentId { get; set; }
        public string productType { get; set; } = string.Empty;
        public string containerType { get; set; } = string.Empty;
        public float shipmentWeight { get; set; } = 0;
        public bool isAccepted { get; set; }
        public bool isActive { get; set; }
        public string checkpoint1Id { get; set; } = string.Empty;
        public string checkpoint2Id { get; set; } = string.Empty;
    }
}
