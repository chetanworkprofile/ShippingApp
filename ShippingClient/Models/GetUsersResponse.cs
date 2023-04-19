namespace ShippingClient.Models
{
    public class GetUsersResponse
    {
        public int statusCode { get; set; }
        public string message { get; set; } = string.Empty;
        public getData data { get; set; } = new getData();
        public bool isSuccess { get; set; }
    }

    public class getData
    {
        public int totalAvailableRecords { get; set; } = 0;
        public List<ResponseUser> list { get; set; } = new();
    }
}
