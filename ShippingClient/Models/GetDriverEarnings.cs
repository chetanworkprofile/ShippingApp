namespace ShippingClient.Models
{
	public class GetDriverEarnings
	{
		public int totalTrips { get; set; }
		public float totalEarnings { get; set; }
		public float monthlyEarning { get; set; }
		public float todayEarning { get; set; }

		public GetDriverEarnings()
		{

		}
	}
}
