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
        public string productType { get; set; }
        public string cointainerType { get; set; }
        public float shipmentPrice { get; set; } = 0;
        public float shipmentWeight { get; set; } = 0;
        public string origin { get; set; }
        public string destination { get; set; }

        public DateTime dateOfOrder { get; set; }
        public Guid shipmentStatusId { get; set; }

        public string notes { get; set; } = string.Empty;
        public string shipmentStatus { get; set; } = string.Empty;
        public string currentLocation { get; set; }
        public DateTime lastUpdated { get; set; } = DateTime.Now;
    }
}
