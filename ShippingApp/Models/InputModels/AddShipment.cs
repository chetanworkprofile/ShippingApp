namespace ShippingApp.Models.InputModels
{
    public class AddShipment
    {
        public Guid productTypeId { get; set; }
        public Guid containerTypeId { get; set; }
        public decimal? shipmentWeight { get; set; }
        public string origin { get; set; } = string.Empty;
        public string destination { get; set; } = string.Empty;
        public string notes { get; set; } = string.Empty;
    }
}
