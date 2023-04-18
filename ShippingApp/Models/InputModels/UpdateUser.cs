namespace ShippingApp.Models.InputModels
{
    public class UpdateUser
    {
        public string firstName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public long contactNo { get; set; } = -1;
        public string address { get; set; } = string.Empty;

        //public string pathToProfilePic { get; set; } = string.Empty;
    }
}

//model to update user details