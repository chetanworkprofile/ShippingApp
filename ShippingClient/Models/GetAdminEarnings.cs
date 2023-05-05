namespace ShippingClient.Models
{
    public class GetAdminEarnings
    {
        public int totalShipment { get; set; }
        public int monthshipment { get; set; }
        public int todayShipment { get; set; }
        public float totalRevenue { get; set; }
        public float monthRevenue { get; set; }
        public float todayRevenue { get; set; }
    }
}
