namespace ShippingClient.Models
{
    public class ShipmentHistory
    {
        public int statusCode { get; set; }
        public string message { get; set; } = string.Empty;
        public List<ShipmentStatusModel> data { get; set; } = new List<ShipmentStatusModel>();
        public bool isSuccess { get; set; }
    }

    public class ShipmentStatusModel
    {
        public Guid shipmentId { get; set; }
        public string shipmentStatus { get; set; } = string.Empty;
        public string currentLocation { get; set; } = string.Empty;
        public DateTime lastUpdated { get; set; }
    }
}
