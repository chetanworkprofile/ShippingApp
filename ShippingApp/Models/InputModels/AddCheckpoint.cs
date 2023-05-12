namespace ShippingApp.Models.InputModels
{
    public class AddCheckpoint
    {
        public string checkpointName { get; set; } = string.Empty;
        public float latitude { get; set; } = 0;
        public float longitude { get; set; } = 0;
        public Guid parentCheckpointId { get; set; }
    }
}
