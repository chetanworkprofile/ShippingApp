namespace ShippingClient.Models
{
    public class AddContainerType
    {
        public string containerName { get; set; } = string.Empty;
        public float price { get; set; } = 0;
        public AddContainerType()
        {

        }
        public AddContainerType(string containerName,float price)
        {
            this.containerName = containerName;
            this.price = price;
        }
    }
}
