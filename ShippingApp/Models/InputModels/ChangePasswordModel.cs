﻿namespace ShippingApp.Models.InputModels
{
    public class ChangePasswordModel
    {
        public string oldPassword { get; set; } = "sjsakld%53677";
        public string newPassword { get; set; } = string.Empty;
    }
}

//model as dto for change password functionality