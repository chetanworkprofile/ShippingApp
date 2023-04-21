using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using ShippingClient.Models;
using ShippingClient.Services;
using ShippingClient.Services.Contracts;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using auth = ShippingClient.AuthProvider;

using static System.Net.WebRequestMethods;
using System.Text.Json;
using System.Text;

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
            //baseUrl = "http://192.180.0.192:5656/";
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
            string savedToken = await _localStorage.GetItemAsync<string>("resetToken");

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}api/v1/resetPassword");
            requestMessage.Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);

            var resetResult = await _httpClient.SendAsync(requestMessage);

            Console.WriteLine("res" + resetResult);
            if (!resetResult.IsSuccessStatusCode)
            {
                var errorResetResponseContent = await resetResult.Content.ReadFromJsonAsync<ErrorLoginResponse>();
                return new LoginResponse { statusCode = 0, message = errorResetResponseContent.message };
            }
            var resetResponseContent = await resetResult.Content.ReadFromJsonAsync<LoginResponse>();
            if (resetResponseContent != null)
            {
                await _localStorage.RemoveItemAsync("resetToken");
                await _localStorage.SetItemAsync("accessToken", resetResponseContent.data.token);
                token = resetResponseContent.data.token;
                ((auth.AuthProvider)_authStateProvider).NotifyUserAuthentication(resetResponseContent.data.token);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", resetResponseContent.data.token);
            }
            return resetResponseContent;

        }

        public async Task<LoginResponse> ChangePassword(ChangePasswordModel model)
        {
            string savedToken = await _localStorage.GetItemAsync<string>("accessToken");

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}api/v1/changePassword");
            requestMessage.Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);

            var changeResult = await _httpClient.SendAsync(requestMessage);

            if (!changeResult.IsSuccessStatusCode)
            {
                var errorResetResponseContent = await changeResult.Content.ReadFromJsonAsync<ErrorLoginResponse>();
                return new LoginResponse { statusCode = 0, message = errorResetResponseContent.message };
            }
            var resetResponseContent = await changeResult.Content.ReadFromJsonAsync<LoginResponse>();
            if (resetResponseContent != null)
            {
                await _localStorage.RemoveItemAsync("accessToken");
                await _localStorage.SetItemAsync("accessToken", resetResponseContent.data.token);
                token = resetResponseContent.data.token;
                ((auth.AuthProvider)_authStateProvider).NotifyUserAuthentication(resetResponseContent.data.token);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", resetResponseContent.data.token);
            }
            return resetResponseContent;

        }

    }
}