﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShippingApp.Models.InputModels;
using ShippingApp.Models.OutputModels;
using ShippingApp.Models;
using System.Data;
using System.Security.Claims;
using ShippingApp.Data;
using ShippingApp.Services;
using ShippingApp.RabbitMQ;

namespace ShippingApp.Controllers
{
    //auth controller to handle all authentication related works like register,login, logout, reset password etc.
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        IAuthService authService;               //service dependency
        Response response = new Response();     //response model instance
        ResponseWithoutData response2 = new ResponseWithoutData();      //response model in case we don't return data
        object result = new object();                                   //object to match both response models in return values from function
        private readonly ILogger<AuthController> _logger;
        //private readonly IValidator<User> _userValidator;

        public AuthController(IConfiguration configuration, ShippingDbContext dbContext, ILogger<AuthController> logger, IMessageProducer messagePublisher)          //constructor
        {
            authService = new AuthService(configuration, dbContext, logger, messagePublisher);
            _logger = logger;
            //_userValidator = validator;
        }

        [HttpPost]
        [Route("/api/v1/user/register")]
        public IActionResult RegisterUser([FromBody] RegisterUser inpUser)             //register user function uses authService to create a new user in db
        {
            //this code is commented out bcz it can be used for fluent validation which is not implemented right now in project

            /*byte[] a = new byte[9302193];
            var user = new User(Guid.NewGuid(), inpUser.firstName, inpUser.lastName, inpUser.email, inpUser.phone, "user", inpUser.address, a, inpUser.pathToProfilePic, "sdasda");
            var validationResult = _userValidator.Validate(user);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new ResponseWithoutData(400, e.ErrorMessage,false)).First();
                return BadRequest(errors);
            }*/
            if (!ModelState.IsValid)
            { 
                //checks for validation of model
                response2 = new ResponseWithoutData(400, "Invalid Input/One or more fields are invalid", false);
                return BadRequest(response2);
            }
            try
            {
                _logger.LogInformation("User registration attempt " + inpUser);
                int statusCode = 0;
                result = authService.CreateUser(inpUser, out statusCode);
                return StatusCode(statusCode, result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal server error ", ex.Message);
                response2 = new ResponseWithoutData(500, $"Internal server error: {ex.Message}", false);
                return StatusCode(500, response2);
            }
        }

        [HttpPost("/api/v1/login")]
        public ActionResult<User> UserLogin(UserDTO request)
        {
            _logger.LogInformation("User Login attempt " + request);
            try
            {
                int statusCode = 0;
                result = authService.Login(request, out statusCode);
                return StatusCode(statusCode, result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal server error ", ex.Message);
                response2 = new ResponseWithoutData(500, $"Internal server error: {ex.Message}", false);
                return StatusCode(500, response2);
            }
        }

        [HttpPost]
        [Route("/api/v1/forgetPassword")]
        public ActionResult<User> ForgetPassword(string Email)
        {
            _logger.LogInformation("forget password attempt");
            try
            {
                int statusCode = 0;
                result = authService.ForgetPassword(Email, out statusCode);
                return StatusCode(statusCode, result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal server error ", ex.Message);
                response2 = new ResponseWithoutData(500, $"Internal server error: {ex.Message}", false);
                return StatusCode(500, response2);
            }
        }

        [HttpPost, Authorize(Roles = "resetpassword")]          // resetpassword is not a role it is to allow only that request which come with resetpassword token and not login token
        [Route("/api/v1/resetPassword")]
        public ActionResult<User> Verify(ResetPasswordModel r)
        {
            _logger.LogInformation("rest password verification attempt");
            try
            {
                string? userId = User.FindFirstValue(ClaimTypes.Sid);                  //extracting userid from token
                int statusCode = 0;
                result = authService.Verify(r, userId, out statusCode);
                return StatusCode(statusCode, result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal server error ", ex.Message);
                response2 = new ResponseWithoutData(500, $"Internal server error: {ex.Message}", false);
                return StatusCode(500, response2);
            }
        }

        [HttpPost, Authorize(Roles = "resetpassword")]
        [Route("/api/v1/driver/setPassword")]
        public ActionResult<User> DriverSetPassword(DriverSetPass r)
        {
            _logger.LogInformation("driver setting new password attempt");
            try
            {
                string? userId = User.FindFirstValue(ClaimTypes.Sid);                  //extracting userid from token
                int statusCode = 0;
                result = authService.SetPassVerify(r, userId, out statusCode);
                return StatusCode(statusCode, result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal server error ", ex.Message);
                response2 = new ResponseWithoutData(500, $"Internal server error: {ex.Message}", false);
                return StatusCode(500, response2);
            }
        }

        [HttpPost, Authorize(Roles = "client, deliveryBoy, manager, admin")]
        [Route("/api/v1/changePassword")]
        public ActionResult<User> ChangePassword(ChangePasswordModel r)
        {
            _logger.LogInformation("change password attempt");
            try
            {
                string token = HttpContext.Request.Headers["Authorization"].FirstOrDefault().Split(" ").Last();        //getting token from header
                var User = HttpContext.User;
                //string email = user.FindFirst(ClaimTypes.Email)?.Value;
                string? userId = User.FindFirstValue(ClaimTypes.Sid);
                int statusCode = 0;
                result = authService.ChangePassword(r, userId, token, out statusCode);
                return StatusCode(statusCode, result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal server error ", ex.Message);
                response2 = new ResponseWithoutData(500, $"Internal server error: {ex.Message}", false);
                return StatusCode(500, response2);
            }
        }

        [HttpPost, Authorize(Roles = "client, deliveryBoy, manager, admin")]
        [Route("/api/v1/logout")]
        public ActionResult<User> Logout()
        {
            _logger.LogInformation("user logout attempt");
            try
            {
                string? userId = User.FindFirstValue(ClaimTypes.Sid);
                string token = HttpContext.Request.Headers["Authorization"].FirstOrDefault().Split(" ").Last();
                int statusCode = 0;
                result = authService.Logout(userId, token, out statusCode);
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
