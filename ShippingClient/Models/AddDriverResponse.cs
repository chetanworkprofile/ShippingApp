namespace ShippingClient.Models
{
    public class AddDriverResponse
    {
        public int statusCode { get; set; }
        public string message { get; set; } = string.Empty;
        public DataDriver data { get; set; } = new DataDriver();
        public bool isSuccess { get; set; }
    }

    public class DataDriver
    {
        public Guid userId { get; set; }
        public string email { get; set; } = "email@shipping.app";
        public Guid checkpointLocation { get; set; }
        public bool isAvailable { get; set; } = true;
    }
}
