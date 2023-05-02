using ShippingClient.Models;

namespace ShippingClient.Services.Contracts
{
    public interface IAuthenticationService
    {
        Task<LoginResponse> Login(LoginDTO model);
        Task<LoginResponse> Register(RegisterUser model);
        Task Logout();
        Task<LoginResponse> ForgetPassword(string email);
        Task<LoginResponse> ResetPassword(ResetPasswordModel model);
        public Task<LoginResponse> DriverSetPassword(DriverSetPassModel model);
        Task<LoginResponse> ChangePassword(ChangePasswordModel model);
        public Task DriverSetPassword(string token);
        string GetToken();
    }
}
