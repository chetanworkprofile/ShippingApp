namespace ShippingClient.Models
{
    public class GetShipmentsCutomerResponse
    {
        public int statusCode { get; set; }
        public string message { get; set; } = string.Empty;
        public List<GetListOfShipments> data { get; set; } = new List<GetListOfShipments>();
        public bool isSuccess { get; set; }
    }
    public class GetListOfShipments
    {
        public Guid shipmentId { get; set; }
        public Guid customerId { get; set; }
        public Guid productTypeId { get; set; }
        public Guid containerTypeId { get; set; }
        public int shipmentPrice { get; set; } = 0;
        public int shipmentWeight { get; set; } = 0;
        public Guid origin { get; set; }
        public Guid destination { get; set; }

        public DateTime dateOfOrder { get; set; }
        public Guid shipmentStatusId { get; set; }

        public string notes { get; set; } = string.Empty;
        public string shipmentStatus { get; set; } = string.Empty;
        public Guid currentLocation { get; set; }
    }
}
