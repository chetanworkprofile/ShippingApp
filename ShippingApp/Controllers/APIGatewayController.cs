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
            _apiGatewayService = new APIGatewayService(dbContext,logger,messagePublisher);
            _logger = logger;
        }

        [HttpGet]
        [Route("/api/v1/getShipments")]
        public ActionResult GetShipments(Guid? shipmentId=null,Guid? customerId = null,Guid? productTypeId = null,Guid? containerTypeId = null)
        {
            _logger.LogInformation("Getting list of shipments");
            try
            {
                return Ok(_apiGatewayService.GetShipments(shipmentId, customerId, productTypeId, containerTypeId));
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal server error ", ex.Message);
                response2 = new ResponseWithoutData(500, $"Internal server error: {ex.Message}", false);
                return StatusCode(500, response2);
            }
        }

        [HttpGet]
        [Route("/api/v1/getProductTypes")]
        public ActionResult GetProductTypes(Guid? productTypeId = null, string? searchString = null)
        {
            _logger.LogInformation("Getting list of productTypes");
            try
            {
                return Ok(_apiGatewayService.GetProductTypes(productTypeId, searchString));
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal server error ", ex.Message);
                response2 = new ResponseWithoutData(500, $"Internal server error: {ex.Message}", false);
                return StatusCode(500, response2);
            }
        }

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
