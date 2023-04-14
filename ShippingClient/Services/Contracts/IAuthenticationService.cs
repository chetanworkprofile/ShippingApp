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
        string GetToken();
    }
}
