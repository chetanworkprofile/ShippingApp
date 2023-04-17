using System.Diagnostics;

namespace ShippingClient.Models
{
    public class CreateShipmentResponse
    {
        public int statusCode{ get; set; }
        public string message { get; set; } = string.Empty; 
        public ShipmentData data { get; set; } = new ShipmentData();
        public bool isSuccess { get; set; }
    }

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