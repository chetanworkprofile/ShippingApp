namespace ShippingApp.Models.InputModels
{
    public class RegisterDriver
    {
        public string firstName { get; set; } = "Firstname";
        public string lastName { get; set; } = "Lastname";
        public string email { get; set; } = "email@chatapp.com";
        public long contactno { get; set; } = 9999999999;
        public string url { get; set; }
        public string address { get; set; } = string.Empty;
        public Guid checkpointLocation { get; set; } 
        public bool isAvailable { get; set; } = true;

        //public string pathToProfilePic { get; set; } = string.Empty;

        public RegisterDriver() { }

    }
}


// input model while user registration