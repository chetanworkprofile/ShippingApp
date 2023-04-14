using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using ShippingClient.Models;
using ShippingClient.Services;
using ShippingClient.Services.Contracts;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using auth = ShippingClient.AuthProvider;

using static System.Net.WebRequestMethods;

namespace ShippingClient.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;
        private readonly string baseUrl;
        private string? token { get; set; } = null;

        public AuthenticationService(HttpClient httpClient, AuthenticationStateProvider authStateProvider,
            ILocalStorageService localStorage)
        {
            this._httpClient = httpClient;
            this._authStateProvider = authStateProvider;
            this._localStorage = localStorage;
            baseUrl = "https://localhost:7147/";
        }


        public async Task<LoginResponse> Login(LoginDTO model)
        {

            var loginResult = await _httpClient.PostAsJsonAsync($"{baseUrl}api/v1/login", model);
            if (!loginResult.IsSuccessStatusCode)
            {
                var errorLoginResponseContent = await loginResult.Content.ReadFromJsonAsync<ErrorLoginResponse>();
                return new LoginResponse { statusCode = 0, message = errorLoginResponseContent.message };
            }
            var loginResponseContent = await loginResult.Content.ReadFromJsonAsync<LoginResponse>();
            if (loginResponseContent != null)
            {
                await _localStorage.SetItemAsync("accessToken", loginResponseContent.data.token);
                token = loginResponseContent.data.token;
                ((auth.AuthProvider)_authStateProvider).NotifyUserAuthentication(loginResponseContent.data.token);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResponseContent.data.token);
            }
            return loginResponseContent;

        }

        public async Task<LoginResponse> Register(RegisterUser model)
        {

            var registerResult = await _httpClient.PostAsJsonAsync($"{baseUrl}api/v1/user/register", model);
            if (!registerResult.IsSuccessStatusCode)
            {
                var errorLoginResponseContent = await registerResult.Content.ReadFromJsonAsync<ErrorLoginResponse>();
                return new LoginResponse { statusCode = 0, message = errorLoginResponseContent.message };
            }
            var registerResponseContent = await registerResult.Content.ReadFromJsonAsync<LoginResponse>();
            if (registerResponseContent != null)
            {
                await _localStorage.SetItemAsync("accessToken", registerResponseContent.data.token);
                token = registerResponseContent.data.token;
                ((auth.AuthProvider)_authStateProvider).NotifyUserAuthentication(registerResponseContent.data.token);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", registerResponseContent.data.token);
            }
            return registerResponseContent;

        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("accessToken");
            token = null;
            ((auth.AuthProvider)_authStateProvider).NotifyUserLogout();
            _httpClient.DefaultRequestHeaders.Authorization = null;

        }

        public string GetToken()
        {
            return this.token;
        }

        public async Task<LoginResponse> ForgetPassword(string email)
        {

            var forgetResult = await _httpClient.PostAsJsonAsync($"{baseUrl}api/v1/forgetPassword?Email={email}","");
            if (!forgetResult.IsSuccessStatusCode)
            {
                var errorForgetResponseContent = await forgetResult.Content.ReadFromJsonAsync<ErrorLoginResponse>();
                return new LoginResponse { statusCode = 0, message = errorForgetResponseContent.message };
            }
            var forgetResponseContent = await forgetResult.Content.ReadFromJsonAsync<LoginResponse>();
            if (forgetResponseContent != null)
            {
                await _localStorage.SetItemAsync("resetToken", forgetResponseContent.data.token);
                token = forgetResponseContent.data.token;
                //((auth.AuthProvider)_authStateProvider).NotifyUserAuthentication(forgetResponseContent.data.token);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", forgetResponseContent.data.token);
            }
            return forgetResponseContent;

        }

        public async Task<LoginResponse> ResetPassword(ResetPasswordModel model)
        {

            var resetResult = await _httpClient.PostAsJsonAsync($"{baseUrl}api/v1/resetPassword", model);
            if (!resetResult.IsSuccessStatusCode)
            {
                var errorResetResponseContent = await resetResult.Content.ReadFromJsonAsync<ErrorLoginResponse>();
                return new LoginResponse { statusCode = 0, message = errorResetResponseContent.message };
            }
            var forgetResponseContent = await resetResult.Content.ReadFromJsonAsync<LoginResponse>();
            if (forgetResponseContent != null)
            {
                await _localStorage.RemoveItemAsync("resetToken");
                await _localStorage.SetItemAsync("accessToken", forgetResponseContent.data.token);
                token = forgetResponseContent.data.token;
                ((auth.AuthProvider)_authStateProvider).NotifyUserAuthentication(forgetResponseContent.data.token);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", forgetResponseContent.data.token);
            }
            return forgetResponseContent;

        }

    }
}