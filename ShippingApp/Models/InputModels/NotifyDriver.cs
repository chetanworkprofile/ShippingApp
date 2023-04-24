namespace ShippingApp.Models.InputModels
{
    public class NotifyDriver
    {
        public List<Guid> driverIds { get; set; } = new();
        //public Guid shipmentId { get; set; }
    }
}
