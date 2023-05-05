namespace ShippingClient.Models
{
    public class DataDriverUpdate
    {
        public Guid driverId { get; set; }
        public Guid checkpointLocation { get; set; }
        public bool isAvailable { get; set; } = true;
    }
}
