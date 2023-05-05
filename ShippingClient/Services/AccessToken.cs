/*using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using ShippingClient.Models;
using ShippingClient.Services.Contracts;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using static System.Net.WebRequestMethods;
using System.Reflection;
using System.Net;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace ShippingClient.Services
{
    public class AccessToken 
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly string baseUrl;

        private string? token { get; set; } = null;

        public AccessToken(HttpClient httpClient,
            ILocalStorageService localStorage)
        {
            this._httpClient = httpClient;
            this._localStorage = localStorage;
            //baseUrl = "https://localhost:7147/";
            baseUrl = "http://192.180.0.192:5656/";
        }

        [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        //private HubConnection hubConnection;

        *//*protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var token = authenticationState.User.FindFirst("access_token").Value;

            hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri($"{baseUrl}shippingHub"), options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(token);
                })
                .WithAutomaticReconnect()
                .Build();

            try
            {
                await hubConnection.StartAsync();
                Console.WriteLine("Connection started");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while starting connection: " + ex.Message);
            }
        }

        public override async Task Dispose()
        {
            if (hubConnection != null)
            {
                await hubConnection.DisposeAsync();
            }
        }*/

        /*public ValueTask<AccessTokenResult> RequestAccessToken()
        {
            AuthenticationStateProvider.GetAuthenticationStateAsync();
            var token = new Microsoft.AspNetCore.Components.WebAssembly.Authentication.AccessToken();
            AccessTokenResult result = new AccessTokenResult(AccessTokenResultStatus.Success, token, "");
            return (ValueTask<AccessTokenResult>)result;
        }

        public ValueTask<AccessTokenResult> RequestAccessToken(AccessTokenRequestOptions options)
        {
            throw new NotImplementedException();
        }*//*
    }
}
*/