namespace ShippingClient.Models
{
    public class AddProductType
    {
        public string type { get; set; } = string.Empty;
        public float price { get; set; } = 0;
        public bool isFragile { get; set; } = true;
        public AddProductType()
        {

        }
        public AddProductType(string type,float price, bool isFragile)
        {
            this.type = type;
            this.price = price;
            this.isFragile = isFragile;
        }
    }
}
