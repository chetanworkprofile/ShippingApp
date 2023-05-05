using System.Diagnostics;

namespace ShippingClient.Models
{
    public class ShipmentData
    {
        public Guid customerId { get; set; }
        public Guid productTypeId { get; set; }
        public Guid containerTypeId { get; set; }
        public int shipmentWeight { get; set; } = 0;
        public Guid origin { get; set; }
        public Guid destination { get; set; }
        public string notes { get; set; } = string.Empty;
    }

}