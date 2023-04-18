﻿using ShippingApp.Controllers;
using ShippingApp.Models;
using Microsoft.EntityFrameworkCore;
using ShippingApp.Models.OutputModels;
using ShippingApp.Data;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Object = System.Object;
using ShippingApp.Models.InputModels;
using ShippingApp.Services;
using ShippingApp.RabbitMQ;

namespace ShippingApp.Services
{
    public class UserService: IUserService
    {
        Response response = new Response();
        ResponseWithoutData response2 = new ResponseWithoutData();
        private readonly ShippingDbContext DbContext;
        private readonly IConfiguration _configuration;
        IAuthService authService;
        // secondary service file to make code clean
        SecondaryAuthService _secondaryAuthService;

        public UserService(IConfiguration configuration, ShippingDbContext dbContext, ILogger<AuthController> logger, IMessageProducer messagePublisher)
        {
            this._configuration = configuration;
            DbContext = dbContext;
            authService = new AuthService(configuration, dbContext, logger, messagePublisher);
            _secondaryAuthService = new SecondaryAuthService(configuration);
        }

        public Object GetYourself(string userId, string token, out int code)
        {
            Guid id = new Guid(userId);
            var userLoggedIn = DbContext.Users.Find(id);

            if (userLoggedIn == null || userLoggedIn.isDeleted == true)
            {
                response2 = new ResponseWithoutData(404, "Can't get details user not found", false);
                code = 404;
                return response2;
            }

            if (token != userLoggedIn.token)
            {
                response2 = new ResponseWithoutData(401, "Invalid/expired token. Login First", false);
                code = 401;
                return response2;
            }

            ResponseUser r = new ResponseUser(userLoggedIn);

            response = new Response(200, "User fetched", r, true);
            code = 200;
            return response;
        }

        public Object UpdateUser(string userId, UpdateUser u, string tokenloggedin, out int code)
        {
            Guid id = new Guid(userId);
            User? user = DbContext.Users.Find(id);

            if (tokenloggedin != user.token)
            {
                response2 = new ResponseWithoutData(401, "Invalid/expired token. Login First", false);
                code = 401;
                return response2;
            }

            if (user != null && user.isDeleted == false)
            {
                if (u.firstName != "string" && u.firstName != string.Empty)
                {
                    user.firstName = u.firstName;
                }
                if (u.lastName != "string" && u.lastName != string.Empty)
                {
                    user.lastName = u.lastName;
                }
                if (u.email != "string" && u.email != string.Empty)
                {
                    user.email = u.email;
                }
                if (u.address != "string" && u.address != string.Empty)
                {
                    user.address = u.address;
                }
                if (u.contactNo != -1 && u.contactNo != 0)
                {
                    user.contactNo = u.contactNo;
                }
                
                user.updatedAt = DateTime.Now;
                DbContext.SaveChanges();

                RegistrationLoginResponse data = new RegistrationLoginResponse(user.userId, user.email, user.firstName, user.lastName, user.userRole, user.token);

                /*await DbContext.SaveChangesAsync();*/
                response = new Response(200, "User updated successfully", data, true);
                code= 200;
                return response;
            }
            else
            {
                response2 = new ResponseWithoutData(404, "User not found", false);
                code = 404;
                return response2;
            }
        }

        public Object DeleteUser(string userId, string token, string password, out int code)
        {
            Guid id = new Guid(userId);
            User? user = DbContext.Users.Find(id);
            if (token != user.token)
            {
                response2 = new ResponseWithoutData(401, "Invalid/expired token. Login First", false);
                code = 401;
                return response2;
            }
            byte[] phash = user.passwordHash;
            if (!_secondaryAuthService.VerifyPasswordHash(password, phash))
            {
                response2 = new ResponseWithoutData(400, "Wrong password.", false);
                code = 400;
                return response2;
            }

            if (user != null && user.isDeleted == false)
            {
                user.isDeleted = true;
                user.token = string.Empty;
                DbContext.SaveChanges();

                response2 = new ResponseWithoutData(200, "User deleted successfully", true);
                code = 200;
                return response2;
            }
            else
            {
                response2 = new ResponseWithoutData(404, "User Not found", false);
                code = 404;
                return response2;
            }

        }
    }
}
