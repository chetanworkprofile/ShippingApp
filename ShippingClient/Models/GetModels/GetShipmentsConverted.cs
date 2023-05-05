namespace ShippingClient.Models
{
    public class GetShipmentsConverted
    {
        public Guid shipmentId { get; set; }
        public string productTypeName { get; set; }
        public string containerTypeName { get; set; }
        public int shipmentPrice { get; set; } = 0;
        public int shipmentWeight { get; set; } = 0;
        public string origin { get; set; }
        public string destination { get; set; }
        public DateTime dateOfOrder { get; set; }
        public string notes { get; set; } = string.Empty;
        public string shipmentStatus { get; set; } = string.Empty;
        public string currentLocation { get; set; }

        public GetShipmentsConverted(Guid id, string product, string container, int price, int weight, string origin, string destination, DateTime date, string notes, string shipmentStatus, string current)
        {
            shipmentId = id;
            productTypeName = product;
            containerTypeName = container;
            shipmentPrice = price;
            shipmentWeight = weight;
            this.origin = origin;
            this.destination = destination;
            dateOfOrder = date;
            this.notes = notes;
            this.shipmentStatus = shipmentStatus;
            currentLocation = current;
        }
    }
}
