namespace ShippingClient.Models
{
    public class DriverId
    {
        public Guid driverId { get; set; }
        public Guid checkpointLocation { get; set; }
        public bool isAvailable { get; set; }
    }
}
