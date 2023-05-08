using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using ShippingClient.Models;
using ShippingClient.Services.Contracts;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using System.Net;

namespace ShippingClient.Services
{
    public class APIService : IAPIService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly string baseUrl;
        private readonly string frontUrl;

        public APIService(HttpClient httpClient,
            ILocalStorageService localStorage)
        {
            this._httpClient = httpClient;
            this._localStorage = localStorage;
            //frontUrl = "https://localhost:7004/";
            frontUrl = "http://192.180.0.192:7676/";
            baseUrl = "https://localhost:7147/";
            //baseUrl = "http://192.180.0.192:5656/";
        }

        public async Task<GlobalResponse> GetProductTypes(string? search = null, Guid? productTypeId = null)
        {
            try
            {
                GlobalResponse? productTypes;
                if (productTypeId != null)
                {
                    productTypes = await _httpClient.GetFromJsonAsync<GlobalResponse>($"{baseUrl}api/v1/get/productTypes?productTypeId={productTypeId}");
                    
                }
                else if (search != null)
                {
                    productTypes = await _httpClient.GetFromJsonAsync<GlobalResponse>($"{baseUrl}api/v1/get/productTypes?searchString={search}");
                }
                else
                {
                    productTypes = await _httpClient.GetFromJsonAsync<GlobalResponse>($"{baseUrl}api/v1/get/productTypes");
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

        public async Task<GlobalResponse> GetContainerTypes(string? search = null, Guid? containerTypeId = null)
        {
            try
            {
                GlobalResponse? containerTypes;
                if (containerTypeId != null)
                {
                    containerTypes = await _httpClient.GetFromJsonAsync<GlobalResponse>($"{baseUrl}api/v1/get/containerTypes?containerTypeId={containerTypeId}");
                }
                else if (search != null)
                {
                    containerTypes = await _httpClient.GetFromJsonAsync<GlobalResponse>($"{baseUrl}api/v1/get/containerTypes?searchString={search}");
                }
                else
                {
                    containerTypes = await _httpClient.GetFromJsonAsync<GlobalResponse>($"{baseUrl}api/v1/get/containerTypes");
                }
                return containerTypes!;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public async Task<GlobalResponse> GetCheckpoints()
        {
            try
            {
                var checkpoints = await _httpClient.GetFromJsonAsync<GlobalResponse>($"{baseUrl}api/v1/get/checkpoints");
                return checkpoints!;
            }
            catch (Exception ex)
            {
                //log exception
                Console.WriteLine(ex);
                throw;
            }
        }
        public async Task<GlobalResponse> GetCheckpointsByName(string? name=null)
        {
            try
            {
                GlobalResponse checkpoints;
                if (name != null)
                {
                    checkpoints = await _httpClient.GetFromJsonAsync<GlobalResponse>($"{baseUrl}api/v1/get/checkpoints?checkpointName={name}");
                }
                else
                {
                    checkpoints = await _httpClient.GetFromJsonAsync<GlobalResponse>($"{baseUrl}api/v1/get/checkpoints");
                }
                return checkpoints!;
            }
            catch (Exception ex)
            {
                //log exception
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<List<DriverId>> GetDrivers(Guid driverId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<GlobalResponse>($"{baseUrl}api/v1/get/drivers?driverId={driverId}");
                
                var obj = JsonSerializer.Serialize(response.data);
                var drivers = JsonSerializer.Deserialize<List<DriverId>>(obj);
                return drivers!;
            }
            catch (Exception ex)
            {
                //log exception
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<GlobalResponse> CreateShipment(CreateShipment model)
        {
            string savedToken = await _localStorage.GetItemAsync<string>("accessToken");

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}api/v1/createShipment");
            requestMessage.Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);

            var result = await _httpClient.SendAsync(requestMessage);

            if (!result.IsSuccessStatusCode)
            {
                var errorResponseContent = await result.Content.ReadFromJsonAsync<ErrorLoginResponse>();
                return new GlobalResponse { statusCode = 0, message = errorResponseContent!.message };
            }
            var resultContent = await result.Content.ReadFromJsonAsync<GlobalResponse>();
            
            return resultContent!;

        }
        public async Task<GlobalResponse> AddProductType(AddProductType model)
        {
            string savedToken = await _localStorage.GetItemAsync<string>("accessToken");

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}api/v1/add/productType");
            requestMessage.Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);

            var result = await _httpClient.SendAsync(requestMessage);

            if (!result.IsSuccessStatusCode)
            {
                var errorResponseContent = await result.Content.ReadFromJsonAsync<GlobalResponse>();
                return new GlobalResponse { statusCode = 0, message = errorResponseContent!.message };
            }
            var resultContent = await result.Content.ReadFromJsonAsync<GlobalResponse>();
            
            return resultContent!;
        }

        public async Task<GlobalResponse> GetCost(CreateShipment model)
        {
            string savedToken = await _localStorage.GetItemAsync<string>("accessToken");

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}api/v1/getCost");
            requestMessage.Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);

            var result = await _httpClient.SendAsync(requestMessage);

            if (!result.IsSuccessStatusCode)
            {
                var errorResponseContent = await result.Content.ReadFromJsonAsync<GlobalResponse>();
                return new GlobalResponse { statusCode = 0, message = errorResponseContent!.message };
            }
            var resultContent = await result.Content.ReadFromJsonAsync<GlobalResponse>();

            return resultContent!;
        }

        public async Task<GlobalResponse> AddCheckpoint(AddCheckpoint model)
        {
            string savedToken = await _localStorage.GetItemAsync<string>("accessToken");

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}api/v1/add/checkpoint");
            requestMessage.Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);

            var result = await _httpClient.SendAsync(requestMessage);

            if (!result.IsSuccessStatusCode)
            {
                var errorResponseContent = await result.Content.ReadFromJsonAsync<GlobalResponse>();
                return new GlobalResponse{ statusCode = 0, message = errorResponseContent!.message };
            }
            var resultContent = await result.Content.ReadFromJsonAsync<GlobalResponse>();

            return resultContent!;
        }

        public async Task<GlobalResponse> AddContainerType(AddContainerType model)
        {
            string savedToken = await _localStorage.GetItemAsync<string>("accessToken");

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}api/v1/add/containerType");
            requestMessage.Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);

            var result = await _httpClient.SendAsync(requestMessage);

            if (!result.IsSuccessStatusCode)
            {
                var errorResponseContent = await result.Content.ReadFromJsonAsync<GlobalResponse>();
                return new GlobalResponse { statusCode = 0, message = errorResponseContent!.message };
            }
            var resultContent = await result.Content.ReadFromJsonAsync<GlobalResponse>();
            
            return resultContent!;

        }
        public async Task<GlobalResponse> AddDriver(AddDriver model)
        {
            var temp = model.url;
            Console.WriteLine("modelurl"+model.url);
            model.url = $"{frontUrl}{model.url}/";
            Console.WriteLine("after"+model.url);

            string savedToken = await _localStorage.GetItemAsync<string>("accessToken");

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}api/v1/add/driver");
            requestMessage.Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);

            var result = await _httpClient.SendAsync(requestMessage);
            model.url = temp;
            if (!result.IsSuccessStatusCode)
            {
                var errorResponseContent = await result.Content.ReadFromJsonAsync<ErrorLoginResponse>();
                return new GlobalResponse { statusCode = 0, message = errorResponseContent!.message };
            }
            var resultContent = await result.Content.ReadFromJsonAsync<GlobalResponse>();
            
            
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

        public async Task<GlobalResponse> UpdateDriverLocation(Guid checkpointId)
        {
            string savedToken = await _localStorage.GetItemAsync<string>("accessToken");
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, $"{baseUrl}api/v1/update/driverLocation?checkpointId={checkpointId}");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);

            var result = await _httpClient.SendAsync(requestMessage);

            if (!result.IsSuccessStatusCode)
            {
                var errorResponseContent = await result.Content.ReadFromJsonAsync<ErrorLoginResponse>();
                return new GlobalResponse { statusCode = 0, message = errorResponseContent!.message };
            }
            var resultContent = await result.Content.ReadFromJsonAsync<GlobalResponse>();
            return resultContent!;
        }

        public async Task<GetShipmentsCutomerResponse> GetShipmentsForCustomer(Guid customerId)
        {
            try
            {
                GetShipmentsCutomerResponse? shipments;

                shipments = await _httpClient.GetFromJsonAsync<GetShipmentsCutomerResponse>($"{baseUrl}api/v1/get/shipments?customerId={customerId}");
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

                yourself = await _httpClient.GetFromJsonAsync<GetYourselfResponse>($"{baseUrl}api/v1/user/getYourself");
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
                productTypes = await _httpClient.GetFromJsonAsync<ProductType>($"{baseUrl}api/v1/get/productTypes?productTypeId={id}");
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
                containerTypes = await _httpClient.GetFromJsonAsync<ContainerType>($"{baseUrl}api/v1/get/containerTypes?containerTypeId={id}");
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
                checkpoint = await _httpClient.GetFromJsonAsync<Checkpoints>($"{baseUrl}api/v1/get/checkpoints?checkpointId={id}");
                return checkpoint!;
            }
            catch (Exception ex)
            {
                //log exception
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<GlobalResponse> GetShipmentHistory(Guid shipmentId)
        {
            try
            {
                GlobalResponse? history;
                history = await _httpClient.GetFromJsonAsync<GlobalResponse>($"{baseUrl}api/v1/get/shipmentHistory?shipmentId={shipmentId}");
                return history!;
            }
            catch (Exception ex)
            {
                //log exception
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<GlobalResponse> GetShipmentHistoryDriver()
        {
            try
            {
                //AvailableShipmentsDriver? shipments;
                GlobalResponse response = await _httpClient.GetFromJsonAsync<GlobalResponse>($"{baseUrl}api/v1/get/driver/shipmentHistory");
                /*var obj = JsonSerializer.Serialize(response.data);
                var history = JsonSerializer.Deserialize<GlobalResponse>(obj);*/
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<List<Checkpoints>> GetShortRoute(Guid shipmentId)
        {
            List<Checkpoints> checkpoints = new List<Checkpoints>();
            try
            {
                var result = await _httpClient.GetAsync($"{baseUrl}api/v1/get/bestRoute?shipmentId={shipmentId}");
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var res = await result.Content.ReadFromJsonAsync<GlobalResponse>();
                    var dat = JsonSerializer.Serialize(res!.data!);
                    checkpoints = JsonSerializer.Deserialize<List<Checkpoints>>(dat)!;
                    return checkpoints;
                };
            }
            catch (Exception ex)
            {
                //log exception
                Console.WriteLine(ex);
                throw;
            }
            return checkpoints;
        }

        public async Task<List<AvailableShipmentsDriver>> GetAvailableShipments(Guid checkpointId)
        {
            try
            {
                AvailableShipmentsDriver? shipments;
                var response = await _httpClient.GetFromJsonAsync<GlobalResponse>($"{baseUrl}api/v1/get/availableShipments?checkpointId={checkpointId}");
                var obj = JsonSerializer.Serialize(response.data);
                var drivers = JsonSerializer.Deserialize<List<AvailableShipmentsDriver>>(obj);
                return drivers!;
            }
            catch (Exception ex)
            {
                //log exception
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<GlobalResponse> AcceptShipment(AcceptShipment model)
        {
            string savedToken = await _localStorage.GetItemAsync<string>("accessToken");

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}api/v1/get/acceptShipment");
            requestMessage.Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);

            var result = await _httpClient.SendAsync(requestMessage);

            if (!result.IsSuccessStatusCode)
            {
                var errorResponseContent = await result.Content.ReadFromJsonAsync<ErrorLoginResponse>();
                return new GlobalResponse { statusCode = 0, message = errorResponseContent!.message };
            }
            var resultContent = await result.Content.ReadFromJsonAsync<GlobalResponse>();

            return resultContent!;

        }

        public async Task<GlobalResponse> UpdateProductTypes(UpdateProductType model)
        {
            try
            {
                string savedToken = await _localStorage.GetItemAsync<string>("accessToken");

                var requestMessage = new HttpRequestMessage(HttpMethod.Put, $"{baseUrl}api/v1/update/productType");
                requestMessage.Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);

                var result = await _httpClient.SendAsync(requestMessage);

                if (!result.IsSuccessStatusCode)
                {
                    var errorResponseContent = await result.Content.ReadFromJsonAsync<ErrorLoginResponse>();
                    return new GlobalResponse { statusCode = 0, message = errorResponseContent!.message };
                }
                var resultContent = await result.Content.ReadFromJsonAsync<GlobalResponse>();

                return resultContent!;
            }
            catch (Exception ex)
            {
                //log exception
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<GlobalResponse> UpdateContainerTypes(UpdateContainerType model)
        {
            try
            {
                string savedToken = await _localStorage.GetItemAsync<string>("accessToken");

                var requestMessage = new HttpRequestMessage(HttpMethod.Put, $"{baseUrl}api/v1/update/containerType");
                requestMessage.Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);

                var result = await _httpClient.SendAsync(requestMessage);

                if (!result.IsSuccessStatusCode)
                {
                    var errorResponseContent = await result.Content.ReadFromJsonAsync<ErrorLoginResponse>();
                    return new GlobalResponse { statusCode = 0, message = errorResponseContent!.message };
                }
                var resultContent = await result.Content.ReadFromJsonAsync<GlobalResponse>();

                return resultContent!;
            }
            catch (Exception ex)
            {
                //log exception
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<GlobalResponse> RemoveProductType(Guid productTypeId)
        {
            try
            {
                var response = await _httpClient.DeleteFromJsonAsync<GlobalResponse>($"{baseUrl}api/v1/remove/productType?productTypeId={productTypeId}");

                return response;
            }
            catch (Exception ex)
            {
                //log exception
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<GlobalResponse> RemoveContainerType(Guid containerTypeId)
        {
            try
            {
                var response = await _httpClient.DeleteFromJsonAsync<GlobalResponse>($"{baseUrl}api/v1/remove/containerType?containerTypeId={containerTypeId}");

                return response;
            }
            catch (Exception ex)
            {
                //log exception
                Console.WriteLine(ex);
                throw;
            }
        }

		public async Task<GlobalResponse> UpdateUser(UpdateUser model)
		{
			try
			{
				string savedToken = await _localStorage.GetItemAsync<string>("accessToken");

				var requestMessage = new HttpRequestMessage(HttpMethod.Put, $"{baseUrl}api/v1/user/update");
				requestMessage.Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
				requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);

				var result = await _httpClient.SendAsync(requestMessage);

				if (!result.IsSuccessStatusCode)
				{
					var errorResponseContent = await result.Content.ReadFromJsonAsync<ErrorLoginResponse>();
					return new GlobalResponse { statusCode = 0, message = errorResponseContent!.message };
				}
				var resultContent = await result.Content.ReadFromJsonAsync<GlobalResponse>();

				return resultContent!;
			}
			catch (Exception ex)
			{
				//log exception
				Console.WriteLine(ex);
				throw;
			}
		}

		public async Task<GlobalResponse> DeleteUser(DeleteUser model)
		{
			try
			{
				string savedToken = await _localStorage.GetItemAsync<string>("accessToken");
				var requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"{baseUrl}api/v1/user/delete");
				requestMessage.Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
				requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);

				var result = await _httpClient.SendAsync(requestMessage);

				if (!result.IsSuccessStatusCode)
				{
					var errorResponseContent = await result.Content.ReadFromJsonAsync<ErrorLoginResponse>();
					return new GlobalResponse { statusCode = 0, message = errorResponseContent!.message };
				}
				var resultContent = await result.Content.ReadFromJsonAsync<GlobalResponse>();

				return resultContent!;
			}
			catch (Exception ex)
			{
				//log exception
				Console.WriteLine(ex);
				throw;
			}
		}

		public async Task<GlobalResponse> GetDriverEarnings(Guid driverId)
		{
			try
			{
				GlobalResponse? response;
				response = await _httpClient.GetFromJsonAsync<GlobalResponse>($"{baseUrl}api/v1/get/driverEarnings?driverId={driverId}");
				return response!;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}
		}

        public async Task<GlobalResponse> GetDriverEarningsForChart(Guid driverId)
        {
            try
            {
                GlobalResponse? response;
                response = await _httpClient.GetFromJsonAsync<GlobalResponse>($"{baseUrl}api/v1/get/driverEarningsForChart?driverId={driverId}");
                return response!;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<GlobalResponse> GetAdminEarnings()
        {
            try
            {
                GlobalResponse? response;
                response = await _httpClient.GetFromJsonAsync<GlobalResponse>($"{baseUrl}api/v1/get/adminEarnings");
                return response!;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<GlobalResponse> GetAdminEarningsForChart()
        {
            try
            {
                GlobalResponse? response;
                response = await _httpClient.GetFromJsonAsync<GlobalResponse>($"{baseUrl}api/v1/get/adminEarningsForChart");
                return response!;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        //do something of this func never used and is not declared in interface
        /*public void DoLogout()
        {
            navigationManager.NavigateTo("/logout");
        }*/
    }
}









/*public async Task<LoginResponse> AddManager(AddManager model)
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

}*/