using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;

namespace ShippingClient.Pages
{
    public class Socket
    {
        public static HubConnection? hubConnection;
        protected NavigationManager _navMgr;
        private readonly ILocalStorageService _localStorage;
        private readonly IConfiguration _configuration;

        public string baseUrl = "";
        //public string baseUrl = "http://192.180.0.192:5656/";
        public Socket(NavigationManager NavigationManager, ILocalStorageService localStorage, IConfiguration configuration)
        {
            _navMgr = NavigationManager;
            _localStorage = localStorage;
            _configuration = configuration;
            baseUrl = _configuration.GetSection("urls:baseUrlServer").Value!;
        }
        public async Task Connect()
        {
            if (hubConnection == null)
            {
                hubConnection = new HubConnectionBuilder()
                .WithUrl(_navMgr.ToAbsoluteUri($"{baseUrl}shippingHub"), options =>
                {
                    options.SkipNegotiation = true;
                    options.Transports = HttpTransportType.WebSockets;
                    options.SkipNegotiation = true;
                    //options.AccessTokenProvider = () => Task.FromResult(_authProvider.GetAuthenticationStateAsync);
                    options.AccessTokenProvider = async () =>
                    {
                        //AuthenticationStateProvider authState = authProvider;
                        return await _localStorage.GetItemAsync<string>("accessToken");
                    };
                })
                .Build();
                try
                {
                    await hubConnection.StartAsync();
                    Console.WriteLine("Connection started");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error while starting connection: " + ex);
                }

            }

        }

        public HubConnection GetHubConnection()
        {
            return hubConnection;
        }
    }
}
