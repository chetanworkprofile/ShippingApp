namespace ShippingApp.Models.InputModels
{
    public class AddShipment
    {
        public Guid productTypeId { get; set; }
        public Guid containerTypeId { get; set; }
        public decimal? shipmentWeight { get; set; }
        public Guid origin { get; set; } 
        public Guid destination { get; set; }
        public string notes { get; set; } = string.Empty;
    }
}
