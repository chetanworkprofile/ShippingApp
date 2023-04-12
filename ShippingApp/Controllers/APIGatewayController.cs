using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using ShippingApp.Models.OutputModels;
using System.Text;
using ShippingApp.Services;
using ShippingApp.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;
using ShippingApp.RabbitMQ;
using ShippingApp.Models.InputModels;
using ShippingApp.Models;

namespace ShippingApp.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class APIGatewayController : ControllerBase
    {
        IAPIGatewayService _apiGatewayService;
        Response response = new Response();     //response model instance
        ResponseWithoutData response2 = new ResponseWithoutData();
        private readonly ILogger<APIGatewayController> _logger;
        public APIGatewayController(IConfiguration configuration, ShippingDbContext dbContext, ILogger<APIGatewayController> logger, IMessageProducer messagePublisher)
        {
            _apiGatewayService = new APIGatewayService(configuration, dbContext,logger,messagePublisher);
            _logger = logger;
        }

        [HttpGet]
        [Route("/api/v1/get/shipments")]
        public ActionResult GetShipments(Guid? shipmentId=null,Guid? customerId = null,Guid? productTypeId = null,Guid? containerTypeId = null)
        {
            _logger.LogInformation("Getting list of shipments");
            try
            {
                int statusCode = 0;
                var res = _apiGatewayService.GetShipments(shipmentId, customerId, productTypeId, containerTypeId,out statusCode);
                return StatusCode(statusCode, res);
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal server error ", ex.Message);
                response2 = new ResponseWithoutData(500, $"Internal server error: {ex.Message}", false);
                return StatusCode(500, response2);
            }
        }

        [HttpGet]
        [Route("/api/v1/get/productTypes")]
        public ActionResult GetProductTypes(Guid? productTypeId = null, string? searchString = null)
        {
            _logger.LogInformation("Getting list of productTypes");
            try
            {
                int statusCode = 0;
                var res = _apiGatewayService.GetProductTypes(productTypeId, searchString, out statusCode);
                return StatusCode(statusCode, res);
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal server error ", ex.Message);
                response2 = new ResponseWithoutData(500, $"Internal server error: {ex.Message}", false);
                return StatusCode(500, response2);
            }
        }

        [HttpGet]
        [Route("/api/v1/get/containerTypes")]
        public ActionResult GetContainerTypes(Guid? containerTypeId = null, string? searchString = null)
        {
            _logger.LogInformation("Getting list of productTypes");
            try
            {
                int statusCode = 0;
                var res = _apiGatewayService.GetContainerTypes(containerTypeId, searchString, out statusCode);
                return StatusCode(statusCode, res);
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal server error ", ex.Message);
                response2 = new ResponseWithoutData(500, $"Internal server error: {ex.Message}", false);
                return StatusCode(500, response2);
            }
        }

        [HttpGet]
        [Route("/api/v1/get/drivers")]
        public ActionResult GetDrivers(Guid? driverId = null, string? searchString = null, string? location = null)
        {
            _logger.LogInformation("Getting list of drivers");
            try
            {
                int statusCode = 0;
                var res = _apiGatewayService.GetDrivers(driverId, searchString, location, out statusCode);
                return StatusCode(statusCode, res);
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal server error ", ex.Message);
                response2 = new ResponseWithoutData(500, $"Internal server error: {ex.Message}", false);
                return StatusCode(500, response2);
            }
        }

        [HttpPost]
        [Route("/api/v1/add/productType")]
        public ActionResult AddProductType(AddProductType inpPtype)
        {
            int statusCode = 0;
            var res = _apiGatewayService.AddProductType(inpPtype, out statusCode);
            return StatusCode(statusCode, res);
        }

        [HttpPost]
        [Route("/api/v1/add/containerType")]
        public ActionResult AddContainerType(AddContainerType inpCtype)
        {
            int statusCode = 0;
            var res = _apiGatewayService.AddContainerType(inpCtype, out statusCode);
            return StatusCode(statusCode, res);
        }

        [HttpPost]
        [Route("/api/v1/add/driver")]
        public ActionResult AddDeliveryPerson(RegisterDriver inpUser)
        {
            int statusCode = 0;
            var res = _apiGatewayService.AddDriver(inpUser, out statusCode);
            return StatusCode(statusCode, res);
        }

        //public ActionResult GetCheckpoints(Guid? containerTypeId = null, string? searchString = null)
        //post containertype
        //post producttype
        //post checkpoint

        /*[HttpPost]
        public Author Test2(Author value)
        {
            string responseString = string.Empty;
            Author objAuthor = new Author();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:65346/");//WebApi 1 project URL
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                StringContent content = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
                var response = client.PostAsync("api/default/Test2", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    responseString = response.Content.ReadAsStringAsync().Result;
                    objAuthor = response.Content.ReadAsAsync<Author>().Result;
                }
            }
            return objAuthor;
        }*/
    }
}
