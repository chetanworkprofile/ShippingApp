namespace ShippingClient.Models
{
    public class ResponseUser
    {
        public Guid userId { get; set; }
        public string firstName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public long contactNo { get; set; }
        public string userRole { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        //public string pathToProfilePic { get; set; } = string.Empty;
        public DateTime? createdAt { get; set; }
        public DateTime? updatedAt { get; set; }
        //public bool isActive { get; set; } = false;

        public ResponseUser() { }
        /*public ResponseUser(User user)
        {
            this.userId = user.userId;
            this.firstName = user.firstName;
            this.lastName = user.lastName;
            this.email = user.email;
            this.contactNo = user.contactNo;
            this.userRole = user.userRole;
            this.address = user.address;
            //this.pathToProfilePic = user.pathToProfilePic;
            this.createdAt = user.createdAt;
            this.updatedAt = user.updatedAt;
            //this.isActive = false;
        }*/
    }
}
