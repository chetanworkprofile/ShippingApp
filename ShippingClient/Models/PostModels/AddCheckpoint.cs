namespace ShippingClient.Models
{
    
    public class AddCheckpoint
    {
        public string checkpointName { get; set; } = string.Empty;
        public float latitude { get; set; } = 0;
        public float longitude { get; set; } = 0;

        public AddCheckpoint()
        {

        }
        public AddCheckpoint(string check, float lat, float longi)
        {
            checkpointName= check;
            latitude= lat;
            longitude= longi;
        }
    }
}
