﻿namespace ShippingApp.Models
{
    public class CreateToken
    {
        public Guid userId { get; set; }
        public string firstName { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string role { get; set; } = string.Empty;

        public CreateToken() { }
        public CreateToken(Guid userId, string firstName, string email, string role)
        {
            this.userId = userId;
            this.firstName = firstName;
            this.email = email;
            this.role = role;
        }
    }
}
