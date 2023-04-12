﻿namespace ShippingApp.Models.OutputModels
{
    public class DriverRegistrationResponse
    {
        public Guid userId { get; set; }
        public string email { get; set; } = string.Empty;
        public string firstName = string.Empty;
        public string lastName = string.Empty;
        //public string userRole { get; set; } = string.Empty;
        //public string token { get; set; } = string.Empty;
        public string location { get; set; } = string.Empty;
        public bool isAvailable { get; set; } = true;
        public DriverRegistrationResponse() { }

        public DriverRegistrationResponse(Guid userId, string email, string firstName, string lastName, string location,bool isAvailable)
        {
            this.userId = userId;
            this.email = email;
            this.firstName = firstName;
            this.lastName = lastName;
            //this.userRole = userRole;
            this.location = location;
            this.isAvailable = isAvailable;
        }
    }
}
