using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShippingApp.Data;
using ShippingApp.Models.OutputModels;
using ShippingApp.Models.InputModels;
using ShippingApp.RabbitMQ;
using ShippingApp.Services;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using System.Security.Claims;

namespace ShippingApp.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IMessageProducer _messagePublisher;
        IClientService _clientService;               //service dependency
        Response response = new Response();     //response model instance
        ResponseWithoutData response2 = new ResponseWithoutData();      //response model in case we don't return data
        object result = new object();                                   //object to match both response models in return values from function
        private readonly ILogger<ClientController> _logger;

        //private readonly IValidator<User> _userValidator;

        public ClientController(ShippingDbContext dbContext, ILogger<ClientController> logger, IMessageProducer messagePublisher)          //constructor
        {
            _clientService = new ClientService(dbContext, logger, messagePublisher);
            _messagePublisher = messagePublisher;
            _logger = logger;
        }

        [HttpPost, Authorize(Roles = "client, manager")]
        [Route("/api/v1/createShipment")]
        public IActionResult CreateNewShipment(AddShipment inpShipment)
        {
            try
            {
                //string token = HttpContext.Request.Headers["Authorization"].FirstOrDefault().Split(" ").Last();        //getting token from header
                var User = HttpContext.User;
                string? userId = User.FindFirstValue(ClaimTypes.Sid);
                int statusCode = 0;

                result = _clientService.CreateShipment(userId, inpShipment, out statusCode);
                //_messagePublisher.SendMessage(inpShipment);
                return StatusCode(statusCode, result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal server error ", ex.Message);
                response2 = new ResponseWithoutData(500, $"Internal server error: {ex.Message}", false);
                return StatusCode(500, response2);
            }
        }

        // getshipmenthistory api
    }
}
