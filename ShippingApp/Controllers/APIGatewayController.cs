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
using Microsoft.AspNetCore.Authorization;
using System.Data;

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

        [HttpGet, Authorize(Roles = "client, manager, admin, deliveryBoy")]
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

        //[HttpGet, Authorize(Roles = "client, manager, admin")]
        [HttpGet, Authorize(Roles = "client, manager, admin, deliveryBoy")]
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

        [HttpGet, Authorize(Roles = "client, manager, admin, deliveryBoy")]
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

        [HttpGet, Authorize(Roles = "client, manager, admin, deliveryBoy")]
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

        [HttpGet, Authorize(Roles = "client, manager, admin, deliveryBoy")]
        [Route("/api/v1/get/checkpoints")]
        public ActionResult GetCheckpoints(Guid? checkpointId = null)
        {
            _logger.LogInformation("Getting list of checkpoints");
            try
            {
                int statusCode = 0;
                var res = _apiGatewayService.GetCheckpoints(checkpointId, out statusCode);
                return StatusCode(statusCode, res);
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal server error ", ex.Message);
                response2 = new ResponseWithoutData(500, $"Internal server error: {ex.Message}", false);
                return StatusCode(500, response2);
            }
        }

        [HttpPost, Authorize(Roles = "admin")]
        [Route("/api/v1/add/productType")]
        public ActionResult AddProductType(AddProductType inpPtype)
        {
            int statusCode = 0;
            var res = _apiGatewayService.AddProductType(inpPtype, out statusCode);
            return StatusCode(statusCode, res);
        }

        [HttpPost, Authorize(Roles = "admin")]
        [Route("/api/v1/add/containerType")]
        public ActionResult AddContainerType(AddContainerType inpCtype)
        {
            int statusCode = 0;
            var res = _apiGatewayService.AddContainerType(inpCtype, out statusCode);
            return StatusCode(statusCode, res);
        }

        [HttpPost, Authorize(Roles = "manager, admin")]
        [Route("/api/v1/add/driver")]
        public ActionResult AddDeliveryPerson(RegisterDriver inpUser)
        {
            int statusCode = 0;
            var res = _apiGatewayService.AddDriver(inpUser, out statusCode);
            return StatusCode(statusCode, res);
        }
        
        [HttpPut, Authorize(Roles = "deliveryBoy")]
        [Route("/api/v1/update/driverLocation")]
        public ActionResult UpdateDriverLocation(Guid checkPointId)
        {
            int statusCode = 0;
            string? token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            string? userId = User.FindFirstValue(ClaimTypes.Sid);
            var res = _apiGatewayService.UpdateDriverLocation(userId, token, checkPointId, out statusCode);
            return StatusCode(statusCode, res);
        }

        //public ActionResult GetCheckpoints(Guid? containerTypeId = null, string? searchString = null)
        //post containertype
        //post producttype
        //post checkpoint

    }
}
