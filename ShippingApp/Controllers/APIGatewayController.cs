using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShippingApp.Models.OutputModels;
using ShippingApp.Services;
using ShippingApp.Data;
using System.Security.Claims;
using ShippingApp.RabbitMQ;
using ShippingApp.Models.InputModels;
using ShippingApp.Models;
using Microsoft.AspNetCore.Authorization;


namespace ShippingApp.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class APIGatewayController : ControllerBase
    {
        IAPIGatewayService _apiGatewayService;
        Response response = new Response();     //response model instance
        ResponseWithoutData response2 = new ResponseWithoutData();      // response model 2 in case of no data return
        private readonly ILogger<APIGatewayController> _logger;         // logger instance to log
        public APIGatewayController(IConfiguration configuration, ShippingDbContext dbContext, ILogger<APIGatewayController> logger, IMessageProducer messagePublisher)
        {
            _apiGatewayService = new APIGatewayService(configuration, dbContext, logger, messagePublisher);
            _logger = logger;
        }

        [HttpGet, Authorize(Roles = "client, admin, deliveryBoy")]
        [Route("/api/v1/get/shipments")]
        public ActionResult GetShipments(Guid? shipmentId = null, Guid? customerId = null, Guid? productTypeId = null, Guid? containerTypeId = null)             // get shipments list acc to various parameters 
        {
            _logger.LogInformation("Getting list of shipments");
            try
            {
                int statusCode = 0;         //to get back status code from service
                var res = _apiGatewayService.GetShipments(shipmentId, customerId, productTypeId, containerTypeId, out statusCode);       // call to service function
                return StatusCode(statusCode, res);
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal server error ", ex.Message);
                response2 = new ResponseWithoutData(500, $"Internal server error: {ex.Message}", false);
                return StatusCode(500, response2);
            }
        }

        [HttpGet, Authorize(Roles = "client, admin, deliveryBoy")]
        [Route("/api/v1/get/shipmentHistory")]
        public ActionResult GetShipmentHistory(Guid? shipmentId = null)             //get history of a particular
        {
            _logger.LogInformation("Getting shipment history with shipment id " + shipmentId);
            try
            {
                int statusCode = 0;
                var res = _apiGatewayService.GetShipmentHistory(shipmentId, out statusCode);
                return StatusCode(statusCode, res);
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal server error ", ex.Message);
                response2 = new ResponseWithoutData(500, $"Internal server error: {ex.Message}", false);
                return StatusCode(500, response2);
            }
        }

        [HttpGet, Authorize(Roles = "client, admin, deliveryBoy")]
        [Route("/api/v1/get/bestRoute")]
        public ActionResult GetBestRoute(Guid? shipmentId = null)
        {
            _logger.LogInformation("Getting best route for shipment with id " + shipmentId);
            try
            {
                int statusCode = 0;
                var res = _apiGatewayService.GetBestRoute(shipmentId, out statusCode);
                return StatusCode(statusCode, res);
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal server error ", ex.Message);
                response2 = new ResponseWithoutData(500, $"Internal server error: {ex.Message}", false);
                return StatusCode(500, response2);
            }
        }

        [HttpGet, Authorize(Roles = "client, admin, deliveryBoy")]
        [Route("/api/v1/get/driver/shipmentHistory")]
        public ActionResult GetShipmentHistoryDriver()
        {
            _logger.LogInformation("Getting shipment history for driver");
            try
            {
                int statusCode = 0;
                string? driverId = User.FindFirstValue(ClaimTypes.Sid);         // getting driver's userid
                var res = _apiGatewayService.GetShipmentHistoryDriver(driverId, out statusCode);
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
        [HttpGet, Authorize(Roles = "client, admin, deliveryBoy")]
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

        [HttpGet, Authorize(Roles = "client, admin, deliveryBoy")]
        [Route("/api/v1/get/containerTypes")]
        public ActionResult GetContainerTypes(Guid? containerTypeId = null, string? searchString = null)
        {
            _logger.LogInformation("Getting list of containerTypes");
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

        [HttpGet, Authorize(Roles = "client, admin ,deliveryBoy")]
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

        [HttpGet, Authorize(Roles = "client, admin, deliveryBoy")]
        [Route("/api/v1/get/checkpoints")]
        public ActionResult GetCheckpoints(Guid? checkpointId = null, string? checkpointName = null)
        {
            _logger.LogInformation("Getting list of checkpoints");
            try
            {
                int statusCode = 0;
                var res = _apiGatewayService.GetCheckpoints(checkpointId, checkpointName, out statusCode);
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
            _logger.LogInformation("Adding new productType to database");
            try
            {
                int statusCode = 0;
                var res = _apiGatewayService.AddProductType(inpPtype, out statusCode);
                return StatusCode(statusCode, res);
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal server error ", ex.Message);
                response2 = new ResponseWithoutData(500, $"Internal server error: {ex.Message}", false);
                return StatusCode(500, response2);
            }
        }

        [HttpPost, Authorize(Roles = "admin,client,manager,deliveryBoy")]
        [Route("/api/v1/getCost")]
        public ActionResult GetCost(AddShipment inpDetails)
        {
            _logger.LogInformation("Getting cost of a shipment");
            try
            {
                int statusCode = 0;
                var res = _apiGatewayService.GetCost(inpDetails, out statusCode);
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
        [Route("/api/v1/add/containerType")]
        public ActionResult AddContainerType(AddContainerType inpCtype)
        {
            _logger.LogInformation("Adding new container type");
            try
            {
                int statusCode = 0;
                var res = _apiGatewayService.AddContainerType(inpCtype, out statusCode);
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
        [Route("/api/v1/add/checkpoint")]
        public ActionResult AddCheckpoint(AddCheckpoint inp)
        {
            _logger.LogInformation("Adding new checkpoint");
            try
            {
                int statusCode = 0;
                var res = _apiGatewayService.AddCheckpoint(inp, out statusCode);
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
        [Route("/api/v1/add/driver")]
        public ActionResult AddDeliveryPerson(RegisterDriver inpUser)
        {
            _logger.LogInformation("Adding new delivery person");
            try
            {
                int statusCode = 0;
                var res = _apiGatewayService.AddDriver(inpUser, out statusCode);
                return StatusCode(statusCode, res);
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal server error ", ex.Message);
                response2 = new ResponseWithoutData(500, $"Internal server error: {ex.Message}", false);
                return StatusCode(500, response2);
            }
        }
        
        [HttpPut, Authorize(Roles = "deliveryBoy")]
        [Route("/api/v1/update/driverLocation")]
        public ActionResult UpdateDriverLocation(Guid checkPointId)
        {
            _logger.LogInformation("Updating delivery person location to checkpoint "+ checkPointId);
            try
            {
                int statusCode = 0;
                string? token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();       // getting token from header
                string? userId = User.FindFirstValue(ClaimTypes.Sid);
                var res = _apiGatewayService.UpdateDriverLocation(userId, token, checkPointId, out statusCode);
                return StatusCode(statusCode, res);
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal server error ", ex.Message);
                response2 = new ResponseWithoutData(500, $"Internal server error: {ex.Message}", false);
                return StatusCode(500, response2);
            }
        }

        [HttpGet, Authorize(Roles = "client, admin, deliveryBoy")]
        [Route("/api/v1/get/availableShipments")]
        public ActionResult GetAvailableShipments(Guid checkpointId)
        {
            _logger.LogInformation("Getting available Shipments at checkpoint "+ checkpointId);
            try
            {
                int statusCode = 0;
                var res = _apiGatewayService.GetAvailableShipments(checkpointId, out statusCode);
                return StatusCode(statusCode, res);
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal server error ", ex.Message);
                response2 = new ResponseWithoutData(500, $"Internal server error: {ex.Message}", false);
                return StatusCode(500, response2);
            }
        }

        [HttpPost, Authorize(Roles = "deliveryBoy")]
        [Route("/api/v1/get/acceptShipment")]
        public ActionResult AcceptShipment(AcceptShipment acceptShipment)
        {
            _logger.LogInformation("Acceptng shipment "+ acceptShipment);
            try
            {
                int statusCode = 0;
                var res = _apiGatewayService.AcceptShipment(acceptShipment, out statusCode);
                return StatusCode(statusCode, res);
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal server error ", ex.Message);
                response2 = new ResponseWithoutData(500, $"Internal server error: {ex.Message}", false);
                return StatusCode(500, response2);
            }
        }

        [HttpPut, Authorize(Roles = "admin")]
        [Route("/api/v1/update/productType")]
        public ActionResult UpdateProductType(UpdateProductType model)
        {
            _logger.LogInformation("Updating product type");
            try
            {
                int statusCode = 0;
                string? token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                string? userId = User.FindFirstValue(ClaimTypes.Sid);
                var res = _apiGatewayService.UpdateProductType(userId, token, model, out statusCode);
                return StatusCode(statusCode, res);
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal server error ", ex.Message);
                response2 = new ResponseWithoutData(500, $"Internal server error: {ex.Message}", false);
                return StatusCode(500, response2);
            }
        }

        [HttpPut, Authorize(Roles = "admin")]
        [Route("/api/v1/update/containerType")]
        public ActionResult UpdateContainerType(UpdateContainerType model)
        {
            _logger.LogInformation("Updating Container type");
            try
            {
                int statusCode = 0;
                string? token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                string? userId = User.FindFirstValue(ClaimTypes.Sid);
                var res = _apiGatewayService.UpdateContainerType(userId, token, model, out statusCode);
                return StatusCode(statusCode, res);
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal server error ", ex.Message);
                response2 = new ResponseWithoutData(500, $"Internal server error: {ex.Message}", false);
                return StatusCode(500, response2);
            }
            
        }

        [HttpDelete, Authorize(Roles = "admin")]
        [Route("/api/v1/remove/containerType")]
        public ActionResult RemoveContainerType(Guid containerTypeId)
        {
            _logger.LogInformation("Removing container type");
            try
            {
                int statusCode = 0;
                string? token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                string? userId = User.FindFirstValue(ClaimTypes.Sid);
                var res = _apiGatewayService.RemoveContainerType(userId, token, containerTypeId, out statusCode);
                return StatusCode(statusCode, res);
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal server error ", ex.Message);
                response2 = new ResponseWithoutData(500, $"Internal server error: {ex.Message}", false);
                return StatusCode(500, response2);
            }
        }

        [HttpDelete, Authorize(Roles = "admin")]
        [Route("/api/v1/remove/productType")]
        public ActionResult RemoveProductType(Guid productTypeId)
        {
            _logger.LogInformation("removing product type");
            try
            {
                int statusCode = 0;
                string? token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                string? userId = User.FindFirstValue(ClaimTypes.Sid);
                var res = _apiGatewayService.RemoveProductType(userId, token, productTypeId, out statusCode);
                return StatusCode(statusCode, res);
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal server error ", ex.Message);
                response2 = new ResponseWithoutData(500, $"Internal server error: {ex.Message}", false);
                return StatusCode(500, response2);
            }
        }

		[HttpGet, Authorize(Roles = "admin, deliveryBoy")]
		[Route("/api/v1/get/driverEarnings")]
		public ActionResult GetDriverEarnings(Guid driverId)
		{
			_logger.LogInformation("Getting driver earnings");
			try
			{
				int statusCode = 0;
				var res = _apiGatewayService.GetDriverEarnings(driverId, out statusCode);
				return StatusCode(statusCode, res);
			}
			catch (Exception ex)
			{
				_logger.LogError("Internal server error ", ex.Message);
				response2 = new ResponseWithoutData(500, $"Internal server error: {ex.Message}", false);
				return StatusCode(500, response2);
			}
		}

		[HttpGet, Authorize(Roles = "admin, deliveryBoy")]
		[Route("/api/v1/get/driverEarningsByDate")]
		public ActionResult GetDriverEarningsByDate(Guid driverId, DateTime startDate, DateTime endDate)
		{
			_logger.LogInformation("Getting driver earnings by date");
			try
			{
				int statusCode = 0;
				var res = _apiGatewayService.GetDriverEarningsByDate(driverId,startDate,endDate, out statusCode);
				return StatusCode(statusCode, res);
			}
			catch (Exception ex)
			{
				_logger.LogError("Internal server error ", ex.Message);
				response2 = new ResponseWithoutData(500, $"Internal server error: {ex.Message}", false);
				return StatusCode(500, response2);
			}
		}

		[HttpGet, Authorize(Roles = "admin, deliveryBoy")]
		[Route("/api/v1/get/driverEarningsForChart")]
		public ActionResult GetDriverEarningsForChart(Guid driverId)
		{
			_logger.LogInformation("Getting driver earnings for chart");
			try
			{
				int statusCode = 0;
				var res = _apiGatewayService.GetDriverEarningsForChart(driverId, out statusCode);
				return StatusCode(statusCode, res);
			}
			catch (Exception ex)
			{
				_logger.LogError("Internal server error ", ex.Message);
				response2 = new ResponseWithoutData(500, $"Internal server error: {ex.Message}", false);
				return StatusCode(500, response2);
			}
		}

        [HttpGet, Authorize(Roles = "admin")]
        [Route("/api/v1/get/adminEarnings")]
        public ActionResult GetAdminEarnings()
        {
            _logger.LogInformation("Getting Admin earnings");
            try
            {
                int statusCode = 0;
                var res = _apiGatewayService.GetAdminEarnings(out statusCode);
                return StatusCode(statusCode, res);
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal server error ", ex.Message);
                response2 = new ResponseWithoutData(500, $"Internal server error: {ex.Message}", false);
                return StatusCode(500, response2);
            }
        }

        [HttpGet, Authorize(Roles = "admin")]
        [Route("/api/v1/get/adminEarningsForChart")]
        public ActionResult GetAdminEarningsForChart()
        {
            _logger.LogInformation("Getting Admin earnings for chart");
            try
            {
                int statusCode = 0;
                var res = _apiGatewayService.GetAdminEarningsForChart(out statusCode);
                return StatusCode(statusCode, res);
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal server error ", ex.Message);
                response2 = new ResponseWithoutData(500, $"Internal server error: {ex.Message}", false);
                return StatusCode(500, response2);
            }
        }
    }
}
