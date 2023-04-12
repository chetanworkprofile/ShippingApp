using ShippingApp.Models.OutputModels;

namespace ShippingApp.Models
{
    public class GetDriversResponse
    {
        public int statusCode { get; set; } = 200;
        public string message { get; set; } = "Ok";
        public List<SendAddDriver> data { get; set; } = new List<SendAddDriver>();
        public bool isSuccess { get; set; } = true;

        public GetDriversResponse() { }
        
    }
}
