﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingApp.Data;
using ShippingApp.Models.InputModels;
using ShippingApp.Models.OutputModels;
using ShippingApp.Services;
using System.Security.Claims;

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
        private readonly ILogger<AdminController> _logger;              // logger instance to log output to console or file
        public AdminController(IConfiguration configuration, ShippingDbContext dbContext, ILogger<AdminController> logger)          //constructor
        {
            adminService = new AdminService(configuration, dbContext, logger);
            _logger = logger;
        }

        [HttpDelete, Authorize(Roles = "admin")]
        [Route("/api/v1/admin/removeUser")]
        public IActionResult RemoveUser(string userId)             // controller for admin to remove any user with userId
        {
            try
            {
                _logger.LogInformation("Admin Removing user attempt with id " + userId);
                string? token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();   // getting token from headers
                string? adminId = User.FindFirstValue(ClaimTypes.Sid);              // getting userid from token
                int statusCode = 0;                     //to get back status code from service
                result = adminService.DeleteUser(adminId,userId, token, out statusCode);    // call to service function
                return StatusCode(statusCode, result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal server error ", ex.Message);
                response2 = new ResponseWithoutData(500, $"Internal server error: {ex.Message}", false);
                return StatusCode(500, response2);
            }
        }

        //getusers api to get list of other users and details
        [HttpGet, Authorize(Roles = "admin")]
        [Route("/api/v1/admin/get")]
        public IActionResult GetUsers(Guid? UserId = null, string userType = "all", string? searchString = null, string? Email = null, long Phone = -1, String OrderBy = "Id", int SortOrder = 1, int RecordsPerPage = 1000, int PageNumber = 1)          // sort order   ===   e1 for ascending  -1 for descending
        {
            _logger.LogInformation("Get users method started by admin");
            try
            {
                string? token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                string? userId = User.FindFirstValue(ClaimTypes.Sid);
                int statusCode = 0;
                result = adminService.GetUsers(userId, userType, token, UserId, searchString, Email, Phone, OrderBy, SortOrder, RecordsPerPage, PageNumber, out statusCode);
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






/*[HttpPost, Authorize(Roles = "admin")]
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
}*/