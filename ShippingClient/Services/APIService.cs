using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using ShippingClient.Models;
using ShippingClient.Services.Contracts;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace ShippingClient.Services
{
    public class APIService : IAPIService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;
        private readonly string baseUrl;
        private string? token { get; set; } = null;

        public APIService(HttpClient httpClient, AuthenticationStateProvider authStateProvider,
            ILocalStorageService localStorage)
        {
            this._httpClient = httpClient;
            this._authStateProvider = authStateProvider;
            this._localStorage = localStorage;
            baseUrl = "https://localhost:7147/";
        }

        public async Task<GetProductsResponse> GetProductTypes()
        {
            try
            {
                var productTypes = await _httpClient.GetFromJsonAsync<GetProductsResponse>("api/v1/get/productTypes");
                return productTypes;
            }
            catch (Exception ex)
            {
                //log exception
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<GetContainerTypesResponse> GetContainerTypes()
        {
            try
            {
                var containerTypes = await _httpClient.GetFromJsonAsync<GetContainerTypesResponse>("api/v1/get/containerTypes");
                return containerTypes;
            }
            catch (Exception ex)
            {
                //log exception
                Console.WriteLine(ex);
                throw;
            }
        }
        public async Task<GetCheckpointsResponse> GetCheckpoints()
        {
            try
            {
                var checkpoints = await _httpClient.GetFromJsonAsync<GetCheckpointsResponse>("api/v1/get/checkpoints");
                return checkpoints;
            }
            catch (Exception ex)
            {
                //log exception
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
