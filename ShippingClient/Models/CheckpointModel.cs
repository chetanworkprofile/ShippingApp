namespace ShippingClient.Models
{
    public class CheckpointModel
    {
        public Guid checkpointId { get; set; } = Guid.NewGuid();
        public string checkpointName { get; set; } = string.Empty;
        public float latitude { get; set; }
        public float longitude { get; set; }
    }
}
