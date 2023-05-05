namespace ShippingClient.Models
{
    public class AddContainerTypeResponse
    {
        public int statusCode { get; set; }
        public string message { get; set; } = string.Empty;
        public ContainerType data { get; set; } = new ContainerType();
        public bool isSuccess { get; set; }
    }
}
