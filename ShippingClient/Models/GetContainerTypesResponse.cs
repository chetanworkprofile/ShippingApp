using System.Diagnostics;

namespace ShippingClient.Models
{
    public class GetContainerTypesResponse
    {
        public int statusCode{ get; set; }
        public string message { get; set; } = string.Empty; 
        public List<ContainerType> data { get; set; } = new List<ContainerType>();
        public bool isSuccess { get; set; }
    }
}