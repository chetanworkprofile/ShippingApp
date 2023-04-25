using System.ComponentModel.DataAnnotations;

namespace ShippingClient.Models
{
    public class CreateShipment
    {
        public Guid productTypeId { get; set; }
        public Guid containerTypeId { get; set; }
        [Required]
        [Range(0,25000)]
        public decimal? shipmentWeight { get; set; }
        public Guid origin { get; set; }
        public Guid destination { get; set; }
        [Required]
        public string notes { get; set; } = string.Empty;
    }
}
