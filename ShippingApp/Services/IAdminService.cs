using ShippingApp.Models.InputModels;

namespace ShippingApp.Services
{
    public interface IAdminService
    {
        public Object CreateManager(RegisterUser inpUser, out int code);
        //public Object Login(UserDTO request, out int code);
    }
}
