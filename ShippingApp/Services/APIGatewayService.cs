﻿using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using ShippingApp.Models;
using System.Text;
using ShippingApp.Controllers;
using ShippingApp.Data;
using ShippingApp.Models.OutputModels;
using ShippingApp.RabbitMQ;
using ShippingApp.Models.InputModels;
using System.Text.RegularExpressions;
using System.Text.Json;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using static System.Net.WebRequestMethods;
using static System.Net.Mime.MediaTypeNames;


namespace ShippingApp.Services
{
    public class APIGatewayService : IAPIGatewayService
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private string baseUrlS1;
        private string baseUrlS2;

        Response response = new Response();                             //response models/objects
        ResponseWithoutData response2 = new ResponseWithoutData();             // response model without data
        object result = new object();
        private readonly ShippingDbContext DbContext;
        private readonly ILogger<APIGatewayController> _logger;
        private readonly IMessageProducer _messagePublisher;
        private readonly IConfiguration _configuration;
        SecondaryAuthService _secondaryAuthService;
        public APIGatewayService(IConfiguration configuration, ShippingDbContext dbContext, ILogger<APIGatewayController> logger, IMessageProducer messagePublisher)
        {
            DbContext = dbContext;
            _logger = logger;
            _messagePublisher = messagePublisher;
            _configuration = configuration;
            _secondaryAuthService = new SecondaryAuthService(configuration);        //secondary service to take care of functions like create password, verify password, .. etc

            baseUrlS1 = _configuration.GetSection("BaseUrls:baseurl1").Value!;      //url of server 1 for microservices(s2) 
            baseUrlS2 = _configuration.GetSection("BaseUrls:baseurl2").Value!;      //url of server 2 (s3)

            /*baseUrlS1 = "http://192.180.2.128:4000";
            baseUrlS2 = "http://192.180.0.127:4040";*/
        }

        //this function simply uses other api to fetch and serve data used to get shipments 
        public string GetShipments(Guid? shipmentId, Guid? customerId, Guid? productTypeId, Guid? containerTypeId, out int code)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrlS1);//WebApi 1 project URL
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                StringBuilder appendUrl = new StringBuilder("api/shipment?");
                if (shipmentId != null)
                {
                    appendUrl.Append("&shipmentId=" + shipmentId + "");

                }
                if (customerId != null)
                {
                    appendUrl.Append("&customerId=" + customerId + "");
                }
                if (productTypeId != null)
                {
                    appendUrl.Append("&productTypeId=" + productTypeId + "");
                }
                if (containerTypeId != null)
                {
                    appendUrl.Append("&containerTypeId=" + containerTypeId + "");
                }
                var res = client.GetAsync(appendUrl.ToString()).Result;

                var data = res.Content.ReadAsStringAsync().Result;

                //response = new Response(200, "Shipments list fetched", data, true);
                code = (int)res.StatusCode;
                return data;
            }
        }

        public string GetShipmentHistory(Guid? shipmentId, out int code)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrlS1);//WebApi 1 project URL
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                StringBuilder appendUrl = new StringBuilder("api/shipment/getShipmentStatus?");
                if (shipmentId != null)
                {
                    appendUrl.Append("&shipmentId=" + shipmentId + "");

                }
                var res = client.GetAsync(appendUrl.ToString()).Result;

                var data = res.Content.ReadAsStringAsync().Result;

                //response = new Response(200, "Shipments list fetched", data, true);
                code = (int)res.StatusCode;
                return data;
            }
        }

        public string GetAvailableShipments(Guid checkpointId, out int code)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrlS2);//WebApi 2 project URL
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                StringBuilder appendUrl = new StringBuilder($"api/Driver/GetShippers?checkpointLocation={checkpointId}");

                var res = client.GetAsync(appendUrl.ToString()).Result;

                var data = res.Content.ReadAsStringAsync().Result;

                //response = new Response(200, "Shipments list fetched", data, true);
                code = (int)res.StatusCode;
                return data;
            }
        }

        public string GetBestRoute(Guid? shipmentId, out int code)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrlS1);//WebApi 1 project URL
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                StringBuilder appendUrl = new StringBuilder($"api/shipment/getShipmentRoute?shipmentId={shipmentId}");
                
                var res = client.GetAsync(appendUrl.ToString()).Result;

                var data = res.Content.ReadAsStringAsync().Result;

                //response = new Response(200, "Shipments list fetched", data, true);
                code = (int)res.StatusCode;
                return data;
            }
        }

        public string GetShipmentHistoryDriver(string driverId, out int code)
        {
            Guid driverGuid = new Guid(driverId);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrlS2);//WebApi 1 project URL
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                StringBuilder appendUrl = new StringBuilder($"api/Driver/GetShippers?driverId={driverGuid}");
                
                var res = client.GetAsync(appendUrl.ToString()).Result;

                var data = res.Content.ReadAsStringAsync().Result;

                //response = new Response(200, "Shipments list fetched", data, true);
                code = (int)res.StatusCode;
                return data;
            }
        }

        public string GetCheckpoints(Guid? checkpointId,string? checkpointName, out int code)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrlS1);//WebApi 1 project URL
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                StringBuilder appendUrl = new StringBuilder("api/shipment/getCheckpoint?");
                if (checkpointId != null)
                {
                    appendUrl.Append("&checkpointId=" + checkpointId + "");

                }
                if (checkpointName != null)
                {
                    appendUrl.Append("&checkpointName=" + checkpointName + "");

                }
                var res = client.GetAsync(appendUrl.ToString()).Result;

                var data = res.Content.ReadAsStringAsync().Result;

                //response = new Response(200, "Shipments list fetched", data, true);
                code = (int)res.StatusCode;
                return data;
            }
        }

        public string GetProductTypes(Guid? productTypeId, string? searchString, out int code)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrlS2);//WebApi 2 project URL
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                StringBuilder appendUrl = new StringBuilder("api/ProductType/Search?");
                if (productTypeId != null)
                {
                    appendUrl.Append("&productTypeId=" + productTypeId + "");

                }
                if (searchString != null)
                {
                    appendUrl.Append("&searchString=" + searchString + "");
                }
                
                var res = client.GetAsync(appendUrl.ToString()).Result;

                var data = res.Content.ReadAsStringAsync().Result;

                //response = new Response(200, "Shipments list fetched", data, true);
                code = (int)res.StatusCode;
                return data;
            }
        }

        public string GetContainerTypes(Guid? containerTypeId, string? searchString, out int code)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrlS2);//WebApi 2 project URL
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                StringBuilder appendUrl = new StringBuilder("api/ContainerType/Search?");
                if (containerTypeId != null)
                {
                    appendUrl.Append("&containerTypeId=" + containerTypeId + "");

                }
                if (searchString != null)
                {
                    appendUrl.Append("&searchString=" + searchString + "");
                }

                var res = client.GetAsync(appendUrl.ToString()).Result;

                var data = res.Content.ReadAsStringAsync().Result;

                //response = new Response(200, "Shipments list fetched", data, true);
                code = (int)res.StatusCode;
                return data;
            }
        }

        public Response GetDrivers(Guid? driverId, string? searchString, string? location, out int code)
        {
            _logger.LogInformation("in service method of get drivers");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrlS2);//WebApi 2 project URL
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                StringBuilder appendUrl = new StringBuilder("api/Driver/Search?");
                if (driverId != null)
                {
                    appendUrl.Append("&driverId=" + driverId + "");

                }
                if (location != null)
                {
                    appendUrl.Append("&location=" + location + "");
                }
                //appendUrl.Append("&isAvailable=true");
                var res = client.GetAsync(appendUrl.ToString()).Result;
                _logger.LogInformation("error" + appendUrl.ToString() + " res: "+ res.StatusCode + " " + res.Content.ToString());
                if (!res.IsSuccessStatusCode)
                {
                    _logger.LogInformation("error" + res);
                    code = (int)res.StatusCode;
                    return JsonConvert.DeserializeObject<Response>(res.Content.ReadAsStringAsync().Result);
                }
                //var data = res.Content.ReadAsStringAsync().Result;
                //var data = System.Text.Json.JsonSerializer.Deserialize<GetDriversResponse>(res.ToString());
                var data = JsonConvert.DeserializeObject<GetDriversResponse>(res.Content.ReadAsStringAsync().Result);
                var list = DbContext.Users.AsQueryable();
                if (searchString != null) { list = list.Where(s => EF.Functions.Like(s.firstName, "%" + searchString + "%") || EF.Functions.Like(s.lastName, "%" + searchString + "%") || EF.Functions.Like(s.firstName + " " + s.lastName, "%" + searchString + "%")); }
                List<ListdriversResponseToUser> listOfDrivers = new List<ListdriversResponseToUser>();

                foreach (var driver in data.data)
                {
                    User? a = list.Where(s => s.userId== driver.driverId).FirstOrDefault();
                    if(a == null)
                    {
                        continue;
                    }
                    ListdriversResponseToUser temp = new ListdriversResponseToUser(a, driver.checkpointLocation, driver.isAvailable);
                    listOfDrivers.Add(temp);
                }
                _logger.LogInformation("list of drivers" + listOfDrivers);
                
                response = new Response(200, "Drivers list fetched", listOfDrivers, true);
                code = 200;
                return response;
            }
        }

        public string GetCost(AddShipment inp, out int code)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrlS1);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                StringContent content = new StringContent(JsonConvert.SerializeObject(inp), Encoding.UTF8, "application/json");
                var response = client.PostAsync("api/shipment/getCost", content).Result;
                code = (int)response.StatusCode;
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        public string AddProductType(AddProductType inp, out int code)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrlS2);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                StringContent content = new StringContent(JsonConvert.SerializeObject(inp), Encoding.UTF8, "application/json");
                var response = client.PostAsync("api/ProductType/Add", content).Result;
                /*if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                    //objAuthor = response.Content.ReadAsAsync<Author>().Result;
                }*/
                code = (int)response.StatusCode;
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        public string AcceptShipment(AcceptShipment inp, out int code)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrlS2);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                StringContent content = new StringContent(JsonConvert.SerializeObject(inp), Encoding.UTF8, "application/json");
                var response = client.PutAsync("api/Driver/AcceptShipment", content).Result;
                /*if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                    //objAuthor = response.Content.ReadAsAsync<Author>().Result;
                }*/
                code = (int)response.StatusCode;
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        public string AddContainerType(AddContainerType inp, out int code)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrlS2);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                StringContent content = new StringContent(JsonConvert.SerializeObject(inp), Encoding.UTF8, "application/json");
                var response = client.PostAsync("api/ContainerType/Add", content).Result;
                
                code = (int)response.StatusCode;
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        public string AddCheckpoint(AddCheckpoint inp, out int code)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrlS1);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                StringContent content = new StringContent(JsonConvert.SerializeObject(inp), Encoding.UTF8, "application/json");
                var response = client.PostAsync("api/shipment/addCheckpoint", content).Result;

                code = (int)response.StatusCode;
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        public object AddDriver(RegisterDriver inpUser, out int code)
        {
            var DbUsers = DbContext.Users;
            bool existingUser = DbUsers.Where(u => u.email == inpUser.email).Any();

            if (!existingUser)
            {
                string regexPatternEmail = "^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$";
                if (!Regex.IsMatch(inpUser.email, regexPatternEmail))
                {
                    response2 = new ResponseWithoutData(400, "Please Enter Valid Email", false);
                    code = 400;
                    return response2;
                }
                /*string regexPatternPassword = "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$";
                if (!Regex.IsMatch(inpUser.password, regexPatternPassword))
                {
                    response2 = new ResponseWithoutData(400, "Please Enter Valid Password. Must contain atleast one uppercase letter, one lowercase letter, one number and one special chararcter and must be atleast 8 characters long", false);
                    code = 400;
                    return response2;
                }*/
                string regexPatternPhone = "^[6-9]\\d{9}$";
                if (!Regex.IsMatch(inpUser.contactno.ToString(), regexPatternPhone))
                {
                    response2 = new ResponseWithoutData(400, "Please Enter Valid PhoneNo", false);
                    code = 400;
                    return response2;
                }
                
                var user = new User(Guid.NewGuid(), inpUser.firstName, inpUser.lastName, inpUser.email, inpUser.contactno, inpUser.address, _secondaryAuthService.CreatePasswordHash("Qwy@163$%^&FbGFrtYu"), "pathToPic", "deliveryBoy", "token");
                // generate token used for reseting password can't user this token to login
                var tokenUser = new CreateToken(user.userId, user.firstName, user.email, "resetpassword");
                string returntoken = _secondaryAuthService.CreateToken(tokenUser);

                string subject = "Mail Verification by Shipping Logistics Management System.Please Verify your account";
                var link = $"{inpUser.url}?access_token={returntoken}";
                string button = "<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\">\r\n    <tr>\r\n        <td>\r\n            <table cellspacing=\"0\" cellpadding=\"0\">\r\n                <tr>\r\n                    <td style=\"border-radius: 2px;\" bgcolor=\"#ED2939\">\r\n                        <a href=" + link + " style=\"padding: 8px 12px; border: 1px solid #ED2939;border-radius: 2px;font-family: Helvetica, Arial, sans-serif;font-size: 14px; color: #ffffff;text-decoration: none;font-weight:bold;display: inline-block;\">\r\n                            Verify Yourself\r\n                        </a>\r\n                    </td>\r\n                </tr>\r\n            </table>\r\n        </td>\r\n    </tr>\r\n</table>";
                string body = "Please verify your email.You are added as a new Delivery person in our system .Please Create your password by clicking on this Button " + button;
                //send mail function used to send mail 
                //response2 = _secondaryAuthService.SendEmail(email, otp);
                response2 = _messagePublisher.SendEmail(new SendEmailModel(inpUser.email, subject, body));
                

                SendAddDriver sendToApi = new SendAddDriver(user.userId, inpUser.checkpointLocation, inpUser.isAvailable);
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrlS2);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    StringContent content = new StringContent(JsonConvert.SerializeObject(sendToApi), Encoding.UTF8, "application/json");
                    var apiResponse = client.PostAsync("api/Driver/Add", content).Result;

                    if (!apiResponse.IsSuccessStatusCode)
                    {
                        code = (int)apiResponse.StatusCode;
                        return apiResponse.Content.ReadAsStringAsync().Result;
                    }

                }
                DbContext.Users.Add(user); 
                DbContext.SaveChanges();
                
                //response object
                DriverRegistrationResponse data = new DriverRegistrationResponse(user.userId, user.email, user.firstName, user.lastName, inpUser.checkpointLocation,inpUser.isAvailable);
                response = new Response(200, "Delivery Person added Successfully", data, true);
                _logger.LogInformation("Delivery Person added successfully", data);
                code = 200;
                return response;
            }
            else
            {
                User userFound = DbUsers.Where(u => u.email == inpUser.email).First();
                if (userFound.isDeleted == true)
                {
                    response2 = new ResponseWithoutData(401, "Account with this email is restricted/deleted", false);
                    code = 401;
                    return response2;
                }
                response2 = new ResponseWithoutData(400, "Email already registered please try another", false);
                code = 400;
                return response2;
            }
        }

        public object UpdateDriverLocation(string userId, string token, Guid checkPointId, out int code)
        {
            Guid driverId = new Guid(userId);
            var userLoggedIn = DbContext.Users.Find(driverId);
            if (token != userLoggedIn.token)
            {
                response = new Response(401, "Invalid/expired token. Login First","", false);
                code = 401;
                return response;
            }
            UpdateDriverLocation updateDriver = new UpdateDriverLocation()
            {
                driverId= driverId,
                checkpointLocation = checkPointId,
                isAvailable = false
            };
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrlS2);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                StringContent content = new StringContent(JsonConvert.SerializeObject(updateDriver), Encoding.UTF8, "application/json");
                var apiResponse = client.PutAsync("api/Driver/Update", content).Result;

                code = (int)apiResponse.StatusCode;
                return apiResponse.Content.ReadAsStringAsync().Result;
            }
        }

        public object UpdateProductType(string userId, string token, UpdateProductType model, out int code)
        {
            Guid driverId = new Guid(userId);
            var userLoggedIn = DbContext.Users.Find(driverId);
            if (token != userLoggedIn.token)
            {
                response = new Response(401, "Invalid/expired token. Login First", "", false);
                code = 401;
                return response;
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrlS2);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                var apiResponse = client.PutAsync("api/ProductType/Update", content).Result;
                code = (int)apiResponse.StatusCode;
                return apiResponse.Content.ReadAsStringAsync().Result;
            }
        }

        public object UpdateContainerType(string userId, string token, UpdateContainerType model, out int code)
        {
            Guid driverId = new Guid(userId);
            var userLoggedIn = DbContext.Users.Find(driverId);
            if (token != userLoggedIn.token)
            {
                response = new Response(401, "Invalid/expired token. Login First", "", false);
                code = 401;
                return response;
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrlS2);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                var apiResponse = client.PutAsync("api/ContainerType/Update", content).Result;
                code = (int)apiResponse.StatusCode;
                return apiResponse.Content.ReadAsStringAsync().Result;
            }
        }

        public object RemoveProductType(string userId, string token, Guid productTypeId, out int code)
        {
            Guid driverId = new Guid(userId);
            var userLoggedIn = DbContext.Users.Find(driverId);
            if (token != userLoggedIn.token)
            {
                response = new Response(401, "Invalid/expired token. Login First", "", false);
                code = 401;
                return response;
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrlS2);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var apiResponse = client.DeleteAsync($"api/ProductType/Remove?productTypeId={productTypeId}").Result;
                code = (int)apiResponse.StatusCode;
                return apiResponse.Content.ReadAsStringAsync().Result;
            }
        }

        public object RemoveContainerType(string userId, string token, Guid containerTypeId, out int code)
        {
            Guid driverId = new Guid(userId);
            var userLoggedIn = DbContext.Users.Find(driverId);
            if (token != userLoggedIn.token)
            {
                response = new Response(401, "Invalid/expired token. Login First", "", false);
                code = 401;
                return response;
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrlS2);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var apiResponse = client.DeleteAsync($"api/ContainerType/Remove?containerTypeId={containerTypeId}").Result;
                code = (int)apiResponse.StatusCode;
                return apiResponse.Content.ReadAsStringAsync().Result;
            }
        }

		public string GetDriverEarnings(Guid driverId, out int code)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(baseUrlS2);//WebApi 2 project URL
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				string appendUrl = $"api/Driver/GetDriverEarnings?driverId={driverId}";

				var res = client.GetAsync(appendUrl).Result;

				var data = res.Content.ReadAsStringAsync().Result;
				code = (int)res.StatusCode;
				return data;
			}
		}

		public string GetDriverEarningsForChart(Guid driverId, out int code)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(baseUrlS2);//WebApi 2 project URL
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				string appendUrl = $"api/Driver/GetChartEarnings?driverId={driverId}";

				var res = client.GetAsync(appendUrl).Result;

				var data = res.Content.ReadAsStringAsync().Result;
				code = (int)res.StatusCode;
				return data;
			}
		}

		public string GetDriverEarningsByDate(Guid driverId,DateTime startDate,DateTime endDate, out int code)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(baseUrlS2);//WebApi 2 project URL
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				string appendUrl = $"api/Driver/GetDateEarnings?driverId={driverId}&date1={startDate}&date2={endDate}";

				var res = client.GetAsync(appendUrl).Result;

				var data = res.Content.ReadAsStringAsync().Result;
				code = (int)res.StatusCode;
				return data;
			}
		}

        public string GetAdminEarnings(out int code)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrlS1);//WebApi 1 project URL
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string appendUrl = $"api/shipment/getData";

                var res = client.GetAsync(appendUrl).Result;

                var data = res.Content.ReadAsStringAsync().Result;
                code = (int)res.StatusCode;
                return data;
            }
        }

        public string GetAdminEarningsForChart(out int code)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrlS1);//WebApi 1 project URL
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string appendUrl = $"api/shipment/getChartData";

                var res = client.GetAsync(appendUrl).Result;

                var data = res.Content.ReadAsStringAsync().Result;
                code = (int)res.StatusCode;
                return data;
            }
        }
    }
}