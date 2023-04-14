namespace ShippingApp.Models.OutputModels
{
    public class SendAddDriver
    {
        public Guid driverId { get; set; } = Guid.NewGuid();
        public Guid checkpointLocation { get; set; }
        public bool isAvailable { get; set; } = true;

        public SendAddDriver() { }

        public SendAddDriver(Guid driverId, Guid checkpointLocation, bool isAvailable)
        {
            this.driverId = driverId;
            this.checkpointLocation = checkpointLocation;
            this.isAvailable = isAvailable;
        }
    }
}
