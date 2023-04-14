namespace ShippingApp.Models
{
    public class ListdriversResponseToUser
    {
        public Guid userId { get; set; }
        public string firstName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public long contactNo { get; set; }
        public string address { get; set; } = string.Empty;
        //public string pathToProfilePic { get; set; } = string.Empty;
        // role can be deliveryBoy , manager, client
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public Guid checkpointLocation { get; set; }
        public bool isAvailable { get; set; } = true;

        public ListdriversResponseToUser()
        {

        }
        public ListdriversResponseToUser(User user, Guid checkpointLocation, bool isAvailable)
        {
            this.userId = user.userId;
            this.firstName = user.firstName;
            this.lastName = user.lastName;
            this.email = user.email;
            this.contactNo = user.contactNo;
            this.address = user.address;
            this.createdAt = user.createdAt;
            this.updatedAt = user.updatedAt;
            this.checkpointLocation = checkpointLocation;
            this.isAvailable = isAvailable;
        }
    }
}
