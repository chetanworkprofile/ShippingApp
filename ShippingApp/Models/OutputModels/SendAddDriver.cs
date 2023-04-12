namespace ShippingApp.Models.OutputModels
{
    public class SendAddDriver
    {
        public Guid driverId { get; set; } = Guid.NewGuid();
        public string location { get; set; } = string.Empty;
        public bool isAvailable { get; set; } = true;

        public SendAddDriver() { }

        public SendAddDriver(Guid driverId, string location, bool isAvailable)
        {
            this.driverId = driverId;
            this.location = location;
            this.isAvailable = isAvailable;
        }
    }
}
