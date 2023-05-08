using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShippingApp.Data;
using ShippingApp.Models.OutputModels;
using ShippingApp.Services;
using System.Data;
using System.Security.Claims;

namespace ShippingApp.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UploadFileController : ControllerBase
    {
        IUploadPicService uploadPicServiceInstance;      //service dependency
        private readonly ILogger<UploadFileController> _logger;
        object result = new object();
        ResponseWithoutData response2 = new ResponseWithoutData();
        Response response = new Response();
        public UploadFileController(ILogger<UploadFileController> logger, IConfiguration configuration, ShippingDbContext dbContext)
        {
            uploadPicServiceInstance = new UploadPicService(dbContext);     //service class instance
            _logger = logger;
        }

        [HttpPost, DisableRequestSizeLimit, Authorize(Roles = "client, deliveryBoy, admin")]
        [Route("/api/v1/uploadProfilePic")]
        public IActionResult ProfilePicUploadAsync(IFormFile file)                //[FromForm] FileUpload File
        {
            _logger.LogInformation("Profile Pic Upload method started");
            try
            {
                string? userId = User.FindFirstValue(ClaimTypes.Sid);           //getting user id from token
                string? userRole = User.FindFirstValue(ClaimTypes.Role);        //getting user role
                string? token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                int statusCode = 0;
                result = uploadPicServiceInstance.ProfilePicUpload(file, userId, token, userRole, out statusCode);

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
