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
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        IPaymentService _paymentService;               //service dependency
        Response response = new Response();     //response model instance
        ResponseWithoutData response2 = new ResponseWithoutData();      //response model in case we don't return data
        object result = new object();                                   //object to match both response models in return values from function
        private readonly ILogger<PaymentController> _logger;              // logger instance to log output to console or file
        public PaymentController(IConfiguration configuration, ShippingDbContext dbContext, ILogger<PaymentController> logger)          //constructor
        {
            _paymentService = new PaymentService(configuration, dbContext, logger);
            _logger = logger;
        }

        [HttpPost, Authorize(Roles = "client,admin")]
        [Route("/api/v1/user/createPaymentOrderId")]
        public IActionResult CreatePaymentOrderId(int amount)             // controller for admin to remove any user with userId
        {
            try
            {
                _logger.LogInformation("Creating new order in razorpay");
                string? token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();   // getting token from headers
                string? userId = User.FindFirstValue(ClaimTypes.Sid);              // getting userid from token
                int statusCode = 0;                     //to get back status code from service
                result = _paymentService.CreateOrder(amount,userId, token, out statusCode);    // call to service function
                return StatusCode(statusCode, result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal server error ", ex.Message);
                response2 = new ResponseWithoutData(500, $"Internal server error: {ex.Message}", false);
                return StatusCode(500, response2);
            }
        }

        [HttpPost, Authorize(Roles = "client,admin")]
        [Route("/api/v1/user/verifyPayment")]
        public IActionResult VerifyPayment(string paymentId, string orderId, string signature)             // controller for admin to remove any user with userId
        {
            try
            {
                _logger.LogInformation("Verifying order in razorpay");
                string? token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();   // getting token from headers
                string? userId = User.FindFirstValue(ClaimTypes.Sid);              // getting userid from token
                int statusCode = 0;                     //to get back status code from service
                result = _paymentService.VerifyPayment(paymentId, orderId, signature, userId, token, out statusCode);    // call to service function
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
