using Blazored.LocalStorage;
using Blazored.LocalStorage.StorageOptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using ShippingClient;
using ShippingClient.AuthProvider;
using ShippingClient.Services;
using ShippingClient.Services.Contracts;
using System.Runtime.CompilerServices;
using static System.Net.WebRequestMethods;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
var baseUrl = "https://localhost:7147/";
//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseUrl) });

builder.Services.AddScoped<AuthenticationStateProvider, AuthProvider>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IAPIService, APIService>();
builder.Services.AddScoped<IAlertService, AlertService>();
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();

//AuthenticationStateProvider _authProvider;
var hubConnection = new HubConnectionBuilder()
    .WithUrl(baseUrl + "shippingHub", options =>
    {
        options.SkipNegotiation = true;
        options.Transports = HttpTransportType.WebSockets;
        options.SkipNegotiation = true;
        //options.AccessTokenProvider = () => Task.FromResult(_authProvider.GetAuthenticationStateAsync);
        /*options.AccessTokenProvider = async () =>
        {
            AuthenticationStateProvider authState = await 
            return authState.User.Claims.FirstOrDefault(c => c.Type == "access_token")?.Value;
        };*/
    })
    .WithAutomaticReconnect()
    .Build();


builder.Services.AddSingleton(hubConnection);

await builder.Build().RunAsync();
