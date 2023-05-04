namespace ShippingClient.Models
{
    public class UpdateUser
    {
        public string firstName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public long contactNo { get; set; } = -1;
        public string address { get; set; } = string.Empty;

        public UpdateUser(ResponseUser user)
        {
            firstName = user.firstName;
            lastName = user.lastName;
            email = user.email;
            contactNo = user.contactNo;
            address = user.address;
        }

        //public string pathToProfilePic { get; set; } = string.Empty;
    }
}

//model to update user details