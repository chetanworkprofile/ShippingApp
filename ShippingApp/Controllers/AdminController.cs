using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingApp.Data;
using ShippingApp.Models.InputModels;
using ShippingApp.Models.OutputModels;
using ShippingApp.Services;

namespace ShippingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        IAdminService adminService;               //service dependency
        Response response = new Response();     //response model instance
        ResponseWithoutData response2 = new ResponseWithoutData();      //response model in case we don't return data
        object result = new object();                                   //object to match both response models in return values from function
        private readonly ILogger<AdminController> _logger;
        public AdminController(IConfiguration configuration, ShippingDbContext dbContext, ILogger<AdminController> logger)          //constructor
        {
            adminService = new AdminService(configuration, dbContext, logger);
            _logger = logger;
        }

        [HttpPost, Authorize]
        [Route("/api/v1/admin/addManager")]
        public IActionResult RegisterManager([FromBody] RegisterUser inpUser)             //register user function uses authService to create a new user in db
        {
            try
            {
                _logger.LogInformation("User registration attempt");
                int statusCode = 0;
                result = adminService.CreateManager(inpUser, out statusCode);
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
