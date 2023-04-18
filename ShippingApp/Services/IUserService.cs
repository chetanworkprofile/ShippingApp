﻿using ShippingApp.Models.InputModels;

namespace ShippingApp.Services
{
    public interface IUserService
    {
        public Object GetYourself(string userId, string token, out int code);
        public Object UpdateUser(string userId, UpdateUser u, string token, out int code);
        public Object DeleteUser(string userId, string token, string password, out int code);
    }
}
