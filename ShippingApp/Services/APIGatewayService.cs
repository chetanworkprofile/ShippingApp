using System.Net.Http.Headers;
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

namespace ShippingApp.Services
{
    public class APIGatewayService : IAPIGatewayService
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private string baseUrlS1;
        private string baseUrlS2;

        Response response = new Response();                             //response models/objects
        ResponseWithoutData response2 = new ResponseWithoutData();             // model to create token
        object result = new object();
        private readonly ShippingDbContext DbContext;
        private readonly ILogger<APIGatewayController> _logger;
        private readonly IMessageProducer _messagePublisher;
        SecondaryAuthService _secondaryAuthService;
        public APIGatewayService(IConfiguration configuration, ShippingDbContext dbContext, ILogger<APIGatewayController> logger, IMessageProducer messagePublisher)
        {
            DbContext = dbContext;
            _logger = logger;
            _messagePublisher = messagePublisher;
            _secondaryAuthService = new SecondaryAuthService(configuration);
            baseUrlS1 = "http://192.180.2.128:4000";
            baseUrlS2 = "http://192.180.0.127:4040";
        }

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

        public string GetCheckpoints(Guid? checkpointId, out int code)
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
                string regexPatternPassword = "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$";
                if (!Regex.IsMatch(inpUser.password, regexPatternPassword))
                {
                    response2 = new ResponseWithoutData(400, "Please Enter Valid Password. Must contain atleast one uppercase letter, one lowercase letter, one number and one special chararcter and must be atleast 8 characters long", false);
                    code = 400;
                    return response2;
                }
                string regexPatternPhone = "^[6-9]\\d{9}$";
                if (!Regex.IsMatch(inpUser.contactno.ToString(), regexPatternPhone))
                {
                    response2 = new ResponseWithoutData(400, "Please Enter Valid PhoneNo", false);
                    code = 400;
                    return response2;
                }
                
                
                var user = new User(Guid.NewGuid(), inpUser.firstName, inpUser.lastName, inpUser.email, inpUser.contactno, inpUser.address, _secondaryAuthService.CreatePasswordHash(inpUser.password), "pathToPic", "deliveryBoy", "token");
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
    }
}