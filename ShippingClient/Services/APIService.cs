using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using ShippingClient.Models;
using ShippingClient.Services.Contracts;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using static System.Net.WebRequestMethods;
using System.Reflection;

namespace ShippingClient.Services
{
    public class APIService : IAPIService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly string baseUrl;
        private string? token { get; set; } = null;

        public APIService(HttpClient httpClient,
            ILocalStorageService localStorage)
        {
            this._httpClient = httpClient;
            this._localStorage = localStorage;
            baseUrl = "https://localhost:7147/";
        }

        public async Task<GetProductsResponse> GetProductTypes(string? search=null)
        {
            try
            {
                GetProductsResponse? productTypes;
                if (search != null)
                {
                    productTypes = await _httpClient.GetFromJsonAsync<GetProductsResponse>($"api/v1/get/productTypes?searchString={search}");
                }
                else
                {
                    productTypes = await _httpClient.GetFromJsonAsync<GetProductsResponse>("api/v1/get/productTypes");
                }
                return productTypes!;
            }
            catch (Exception ex)
            {
                //log exception
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<GetContainerTypesResponse> GetContainerTypes(string? search = null)
        {
            try
            {
                GetContainerTypesResponse? containerTypes;
                if (search != null)
                {
                    containerTypes = await _httpClient.GetFromJsonAsync<GetContainerTypesResponse>($"api/v1/get/containerTypes?searchString={search}");
                }
                else
                {
                    containerTypes = await _httpClient.GetFromJsonAsync<GetContainerTypesResponse>("api/v1/get/containerTypes");
                }
                return containerTypes!;
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
                return checkpoints!;
            }
            catch (Exception ex)
            {
                //log exception
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<CreateShipmentResponse> CreateShipment(CreateShipment model)
        {
            string savedToken = await _localStorage.GetItemAsync<string>("accessToken");

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}api/v1/createShipment");
            requestMessage.Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);

            var result = await _httpClient.SendAsync(requestMessage);

            if (!result.IsSuccessStatusCode)
            {
                var errorResponseContent = await result.Content.ReadFromJsonAsync<ErrorLoginResponse>();
                return new CreateShipmentResponse { statusCode = 0, message = errorResponseContent!.message };
            }
            var resultContent = await result.Content.ReadFromJsonAsync<CreateShipmentResponse>();
            /*if (resultContent != null)
            {
                await _localStorage.RemoveItemAsync("accessToken");
                await _localStorage.SetItemAsync("accessToken", resultContent.data.token);
                token = resultContent.data.token;
            }*/
            return resultContent!;

        }
        public async Task<AddProductTypeResponse> AddProductType(AddProductType model)
        {
            string savedToken = await _localStorage.GetItemAsync<string>("accessToken");

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}api/v1/add/productType");
            requestMessage.Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);

            var result = await _httpClient.SendAsync(requestMessage);

            if (!result.IsSuccessStatusCode)
            {
                var errorResponseContent = await result.Content.ReadFromJsonAsync<GlobalResponse>();
                return new AddProductTypeResponse { statusCode = 0, message = errorResponseContent!.message };
            }
            var resultContent = await result.Content.ReadFromJsonAsync<AddProductTypeResponse>();
            
            return resultContent!;

        }
        
        public async Task<AddContainerTypeResponse> AddContainerType(AddContainerType model)
        {
            string savedToken = await _localStorage.GetItemAsync<string>("accessToken");

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}api/v1/add/containerType");
            requestMessage.Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);

            var result = await _httpClient.SendAsync(requestMessage);

            if (!result.IsSuccessStatusCode)
            {
                var errorResponseContent = await result.Content.ReadFromJsonAsync<GlobalResponse>();
                return new AddContainerTypeResponse { statusCode = 0, message = errorResponseContent!.message };
            }
            var resultContent = await result.Content.ReadFromJsonAsync<AddContainerTypeResponse>();
            
            return resultContent!;

        }
        public async Task<AddDriverResponse> AddDriver(AddDriver model)
        {
            string savedToken = await _localStorage.GetItemAsync<string>("accessToken");

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}api/v1/add/driver");
            requestMessage.Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);

            var result = await _httpClient.SendAsync(requestMessage);

            if (!result.IsSuccessStatusCode)
            {
                var errorResponseContent = await result.Content.ReadFromJsonAsync<ErrorLoginResponse>();
                return new AddDriverResponse { statusCode = 0, message = errorResponseContent!.message };
            }
            var resultContent = await result.Content.ReadFromJsonAsync<AddDriverResponse>();
            
            return resultContent!;

        }
        
        public async Task<LoginResponse> AddManager(AddManager model)
        {
            string savedToken = await _localStorage.GetItemAsync<string>("accessToken");

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}api/v1/admin/addManager");
            requestMessage.Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);

            var result = await _httpClient.SendAsync(requestMessage);

            if (!result.IsSuccessStatusCode)
            {
                var errorResponseContent = await result.Content.ReadFromJsonAsync<ErrorLoginResponse>();
                return new LoginResponse { statusCode = 0, message = errorResponseContent!.message };
            }
            var resultContent = await result.Content.ReadFromJsonAsync<LoginResponse>();
            
            return resultContent!;

        }

        public async Task<GetUsersResponse> GetUsers(int pageNumber = 1,string? search = null)
        {
            try
            {
                GetUsersResponse? users;
                string url = string.Empty;
                string savedToken = await _localStorage.GetItemAsync<string>("accessToken");
                if (search != null)
                {
                    url = $"api/v1/admin/get?userType=all&Phone=-1&OrderBy=Id&SortOrder=1&RecordsPerPage=10&PageNumber={pageNumber}&searchString={search}";
                }
                else
                {
                    url = $"api/v1/admin/get?userType=all&Phone=-1&OrderBy=Id&SortOrder=1&RecordsPerPage=10&PageNumber={pageNumber}";
                }
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}{url}");
                
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);

                var result = await _httpClient.SendAsync(requestMessage);

                
                if (!result.IsSuccessStatusCode)
                {
                    var errorResponseContent = await result.Content.ReadFromJsonAsync<ErrorLoginResponse>();
                    return new GetUsersResponse { statusCode = 0, message = errorResponseContent!.message };
                }
                var res = await result.Content.ReadFromJsonAsync<GetUsersResponse>()!;
                return res;
            }
            catch (Exception ex)
            {
                //log exception
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<UpdateDriverLocationResponse> UpdateDriverLocation(Guid checkpointId)
        {
            string savedToken = await _localStorage.GetItemAsync<string>("accessToken");
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, $"{baseUrl}api/v1/update/driverLocation?checkpointId={checkpointId}");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);

            var result = await _httpClient.SendAsync(requestMessage);

            if (!result.IsSuccessStatusCode)
            {
                var errorResponseContent = await result.Content.ReadFromJsonAsync<ErrorLoginResponse>();
                return new UpdateDriverLocationResponse { statusCode = 0, message = errorResponseContent!.message };
            }
            var resultContent = await result.Content.ReadFromJsonAsync<UpdateDriverLocationResponse>();
            return resultContent!;
        }

        public async Task<GetShipmentsCutomerResponse> GetShipmentsForCustomer(Guid customerId)
        {
            try
            {
                GetShipmentsCutomerResponse? shipments;

                shipments = await _httpClient.GetFromJsonAsync<GetShipmentsCutomerResponse>($"api/v1/get/shipments?customerId={customerId}");
                return shipments!;
            }
            catch (Exception ex)
            {
                //log exception
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<GetYourselfResponse> GetYourself()
        {
            try
            {
                GetYourselfResponse? yourself;

                yourself = await _httpClient.GetFromJsonAsync<GetYourselfResponse>("api/v1/user/getYourself");
                return yourself!;
            }
            catch (Exception ex)
            {
                //log exception
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<ProductType> GetProductTypeSingle(Guid id)
        {
            try
            {
                ProductType? productTypes;
                productTypes = await _httpClient.GetFromJsonAsync<ProductType>($"api/v1/get/productTypes?productTypeId={id}");
                return productTypes!;
            }
            catch (Exception ex)
            {
                //log exception
                Console.WriteLine(ex);
                throw;
            }
        }
        public async Task<ContainerType> GetContainerTypeSingle(Guid id)
        {
            try
            {
                ContainerType? containerTypes;
                containerTypes = await _httpClient.GetFromJsonAsync<ContainerType>($"api/v1/get/containerTypes?containerTypeId={id}");
                return containerTypes!;
            }
            catch (Exception ex)
            {
                //log exception
                Console.WriteLine(ex);
                throw;
            }
        }
        public async Task<Checkpoints> GetCheckpointTypeSingle(Guid id)
        {
            try
            {
                Checkpoints? checkpoint;
                checkpoint = await _httpClient.GetFromJsonAsync<Checkpoints>($"api/v1/get/checkpoints?checkpointId={id}");
                return checkpoint!;
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
