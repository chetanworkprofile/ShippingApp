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
                ((auth.AuthProvider)_authStateProvider).NotifyUserAuthentication(loginResponseContent.data.token);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResponseContent.data.token);
            }
            return loginResponseContent;

        }

        public async Task<LoginResponse> Register(RegisterUser model)
        {

            var loginResult = await _httpClient.PostAsJsonAsync($"{baseUrl}api/v1/register", model);
            if (!loginResult.IsSuccessStatusCode)
            {
                var errorLoginResponseContent = await loginResult.Content.ReadFromJsonAsync<ErrorLoginResponse>();
                return new LoginResponse { statusCode = 0, message = errorLoginResponseContent.message };
            }
            var loginResponseContent = await loginResult.Content.ReadFromJsonAsync<LoginResponse>();
            if (loginResponseContent != null)
            {
                await _localStorage.SetItemAsync("accessToken", loginResponseContent.data.token);
                ((auth.AuthProvider)_authStateProvider).NotifyUserAuthentication(loginResponseContent.data.token);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResponseContent.data.token);
            }
            return loginResponseContent;

        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("accessToken");
            ((auth.AuthProvider)_authStateProvider).NotifyUserLogout();
            _httpClient.DefaultRequestHeaders.Authorization = null;

        }

    }
}