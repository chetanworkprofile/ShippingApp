namespace ShippingApp.Models
{
    public class User
    {
        public Guid userId { get; set; }
        public string firstName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public long contactNo { get; set; }
        public string address { get; set; } = string.Empty;
        public byte[] passwordHash { get; set; } = new byte[32];
        public string userRole { get; set; } = string.Empty;
        public string pathToProfilePic { get; set; } = string.Empty;
        // role can be deliveryBoy , manager, client
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public string token { get; set; } = string.Empty;
        public int? verificationOTP { get; set; }
        public DateTime otpUsableTill { get; set; }
        public DateTime? verifiedAt { get; set; }
        public bool isDeleted { get; set; }

        public User() { }
        public User(Guid userId, string firstName, string lastName, string email, long contactNo, string address, byte[] passwordHash, string pathToProfilePic, string userRole, string token)
        {
            this.userId = userId;
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;
            this.contactNo = contactNo;
            this.address = address;
            this.passwordHash = passwordHash;
            this.pathToProfilePic = pathToProfilePic;
            this.userRole = userRole;
            createdAt = DateTime.Now;
            updatedAt = DateTime.Now;
            this.token = token;
            verificationOTP = 999;
            otpUsableTill = DateTime.Now;
            verifiedAt = DateTime.Now;
            isDeleted = false;
        }
    }
}
