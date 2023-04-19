namespace ShippingApp.Models.OutputModels
{
    public class UpdateDriverLocation
    {
        public Guid driverId { get; set; }
        public Guid checkpointLocation { get; set; }
        public bool isAvailable { get; set; }  = false;
    }
}
