using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShippingApp.Data;
using ShippingApp.Models.InputModels;
using ShippingApp.Models.OutputModels;
using ShippingApp.RabbitMQ;
using ShippingApp.Services;
using System.Data;
using System.Security.Claims;

namespace ShippingApp.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserService userService;                   //service dependency
        Response response = new Response();         //response model
        ResponseWithoutData response2 = new ResponseWithoutData();      //response model without data
        object result = new object();                   //to accomodate both response models in function return type
        private readonly ILogger<AuthController> _logger;

        public UserController(IConfiguration configuration, ShippingDbContext dbContext, ILogger<AuthController> logger, IMessageProducer messageProducer)
        {
            userService = new UserService(configuration, dbContext, logger, messageProducer);
            _logger = logger;
        }

        [HttpGet, Authorize(Roles = "admin,client,deliveryBoy")]
        [Route("/api/v1/user/getYourself")]
        public IActionResult GetYourself()                  // api for user to get data of himself for proifile details
        {
            _logger.LogInformation("Get yourself method started");
            try
            {
                string? token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                string? userId = User.FindFirstValue(ClaimTypes.Sid);
                int statusCode = 0;
                result = userService.GetYourself(userId, token,out statusCode);
                return StatusCode(statusCode, result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal server error ", ex.Message);
                response2 = new ResponseWithoutData(500, $"Internal server error: {ex.Message}", false);
                return StatusCode(500, response2);
            }
        }

        [HttpPut, Authorize(Roles = "admin,client,deliveryBoy")]
        [Route("/api/v1/user/update")]
        public IActionResult UpdateUser(UpdateUser u)               //api to update user's profile
        {
            _logger.LogInformation("Update user method started");
            try
            {
                string? userId = User.FindFirstValue(ClaimTypes.Sid);
                string? token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                int statusCode = 0;
                result = userService.UpdateUser(userId, u, token, out statusCode);

                return StatusCode(statusCode, result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal server error ", ex.Message);
                response2 = new ResponseWithoutData(500, $"Internal server error: {ex.Message}", false);
                return StatusCode(500, response2);
            }
        }

        [HttpDelete, Authorize(Roles = "client,deliveryBoy")]
        [Route("/api/v1/user/delete")]
        public IActionResult DeleteUser(DeleteUser user)            //method for user to delete his/her account
        {
            _logger.LogInformation("Delete user method started");
            try
            {
                string? userId = User.FindFirstValue(ClaimTypes.Sid);
                string? token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                int statusCode = 0;
                result = userService.DeleteUser(userId, token, user.password, out statusCode);
                return StatusCode(statusCode, result);
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
