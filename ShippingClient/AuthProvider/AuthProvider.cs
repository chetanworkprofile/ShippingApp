﻿using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace ShippingClient.AuthProvider
{
        public class AuthProvider : AuthenticationStateProvider
        {
            private readonly HttpClient _http;
            private readonly ILocalStorageService _localStorage;
            private readonly AuthenticationState _anonymous;
/*            public string baseUrl = "https://localhost:7147/";
            //public string baseUrl = "http://192.180.0.192:5656/"*/
            public AuthProvider(HttpClient http, ILocalStorageService localStorage)
            {
                _http = http;
                _localStorage = localStorage;
                _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            public override async Task<AuthenticationState> GetAuthenticationStateAsync()
            {
                var token = await _localStorage.GetItemAsync<string>("accessToken");
                if (string.IsNullOrEmpty(token))
                {
                    return _anonymous;
                }
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                return new AuthenticationState(new ClaimsPrincipal(
                      new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwtAuthType")));
            }

            public void NotifyUserAuthentication(string token)
            {
                var authUser = new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwtAuthType"));
                var authState = Task.FromResult(new AuthenticationState(authUser));
                NotifyAuthenticationStateChanged(authState);

            }

            public void NotifyUserLogout()
            {
                var authState = Task.FromResult(_anonymous);
                NotifyAuthenticationStateChanged(authState);
            }

            /*public async void StartHub(HubConnection hubConnection)
            {
                hubConnection = new HubConnectionBuilder()
                .WithUrl(baseUrl + "shippingHub", options =>
                {
                    options.SkipNegotiation = true;
                    options.Transports = HttpTransportType.WebSockets;
                    options.SkipNegotiation = true;
                    //options.AccessTokenProvider = () => Task.FromResult(_authProvider.GetAuthenticationStateAsync);
                    options.AccessTokenProvider = async () =>
                    {
                        //AuthenticationStateProvider authState = GetAuthenticationStateAsync().Result;
                        return _anonymous.User.Claims.FirstOrDefault(c => c.Type == "access_token")?.Value;
                    };
                })
                .WithAutomaticReconnect()
                .Build();

            }

            public async void DisposeHub(HubConnection hubConnection)
            {
                if (hubConnection != null)
                {
                    await hubConnection.DisposeAsync();
                }
            }*/

        }
}
