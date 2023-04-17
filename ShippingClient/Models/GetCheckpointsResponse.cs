using System.Diagnostics;

namespace ShippingClient.Models
{
    public class GetCheckpointsResponse
    {
        public int statusCode{ get; set; }
        public string message { get; set; } = string.Empty; 
        public List<Checkpoints> data { get; set; } = new List<Checkpoints>();
        public bool isSuccess { get; set; }
    }
}