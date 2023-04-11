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
        //IAuthService authService;               //service dependency
        Response response = new Response();     //response model instance
        ResponseWithoutData response2 = new ResponseWithoutData();      //response model in case we don't return data
        object result = new object();                                   //object to match both response models in return values from function
        private readonly ILogger<ClientController> _logger;

        //private readonly IValidator<User> _userValidator;

        public ClientController(IConfiguration configuration, ShippingDbContext dbContext, ILogger<ClientController> logger, IMessageProducer messagePublisher)          //constructor
        {
            //authService = new AuthService(configuration, dbContext, logger);
            _messagePublisher = messagePublisher;
            _logger = logger;
        }

        [HttpPost, Authorize(Roles = "client")]
        [Route("/api/v1/createShipment")]
        public IActionResult CreateNewShipment(AddShipment inpShipment)
        {
            string token = HttpContext.Request.Headers["Authorization"].FirstOrDefault().Split(" ").Last();        //getting token from header
            /*var user = HttpContext.User;
            string email = user.FindFirst(ClaimTypes.Email)?.Value;*/
            string? userId = User.FindFirstValue(ClaimTypes.Sid);

            _messagePublisher.SendMessage(inpShipment);
            response = new Response(200,"sent","no data",true);
            return Ok(response);
        }
    }
}
