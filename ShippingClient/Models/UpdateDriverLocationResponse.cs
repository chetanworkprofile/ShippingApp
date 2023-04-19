namespace ShippingClient.Models
{
    public class UpdateDriverLocationResponse
    {
        public int statusCode { get; set; }
        public string message { get; set; } = string.Empty;
        public DataDriverUpdate data { get; set; } = new DataDriverUpdate();
        public bool isSuccess { get; set; }
    }

    public class DataDriverUpdate
    {
        public Guid driverId { get; set; }
        public Guid checkpointLocation { get; set; }
        public bool isAvailable { get; set; } = true;
    }
}
