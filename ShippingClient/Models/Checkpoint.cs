namespace ShippingClient.Models
{
    public class Checkpoints
    {
        public Guid checkpointId { get; set; }
        public string checkpointName { get; set; } = string.Empty;
        public float latitude { get; set; }
        public float longitude { get; set; }
    }
}
