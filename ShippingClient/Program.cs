using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ShippingClient;
using ShippingClient.AuthProvider;
using ShippingClient.Pages;
using ShippingClient.Services;
using ShippingClient.Services.Contracts;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
//var baseUrl = "http://192.180.0.192:5656/";
var baseUrl = "https://localhost:7147/";
//var baseUrl = builder.Configuration.GetSection("urls:baseUrlServer").Value;
//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseUrl) });

builder.Services.AddScoped<AuthenticationStateProvider, AuthProvider>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IAPIService, APIService>();
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddSingleton<Socket>();
builder.Services.AddMudServices();

/*
builder.Configuration.GetConnectionString("ConnectionString")*/

await builder.Build().RunAsync();
