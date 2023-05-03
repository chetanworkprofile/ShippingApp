using ShippingApp.Models.InputModels;

namespace ShippingApp.Services
{
    public interface IAdminService
    {
        //public Object CreateManager(RegisterUser inpUser, out int code);
        //public Object Login(UserDTO request, out int code);
        public Object GetUsers(string userId, string userType, string token, Guid? UserId, string? searchString, string? Email, long Phone, string OrderBy, int SortOrder, int RecordsPerPage, int PageNumber, out int code);
        public Object DeleteUser(string adminId,string userId, string token, out int code);
    }
}
