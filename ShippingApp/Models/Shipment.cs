using ShippingApp.Models.InputModels;

namespace ShippingApp.Models
{
    public class Shipment
    {
        public Guid customerId { get; set; }
        public Guid productTypeId { get; set; }
        public Guid containerTypeId { get; set; }
        public decimal? shipmentWeight { get; set; }
        public string origin { get; set; } = string.Empty;
        public string destination { get; set; } = string.Empty;
        public string notes { get; set; } = string.Empty;

        public Shipment(){} 

        public Shipment(Guid id,AddShipment a)
        {
            customerId= id;
            productTypeId = a.productTypeId;
            containerTypeId = a.containerTypeId;
            shipmentWeight = a.shipmentWeight;
            origin = a.origin;
            destination = a.destination;
            notes = a.notes;
        }
    }
}
