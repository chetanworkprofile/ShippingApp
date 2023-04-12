using ShippingApp.Controllers;
using ShippingApp.Data;
using ShippingApp.Models.OutputModels;
using ShippingApp.Models;
using ShippingApp.RabbitMQ;
using ShippingApp.Models.InputModels;
using System.Text.RegularExpressions;
using System.Configuration;

namespace ShippingApp.Services
{
    public class AdminService : IAdminService
    {
        Response response = new Response();                             //response models/objects
        ResponseWithoutData response2 = new ResponseWithoutData();      // response model without data field

        object result = new object();
        private readonly ShippingDbContext DbContext;           // entity framework dbcontext
        private readonly ILogger<AdminController> _logger;       // for logging
        // secondary service file to make code clean
        SecondaryAuthService _secondaryAuthService;

        public AdminService(IConfiguration configuration, ShippingDbContext dbContext, ILogger<AdminController> logger)
        {
            DbContext = dbContext;
            _logger = logger;
            _secondaryAuthService = new SecondaryAuthService(configuration);
        }

        public Object CreateManager(RegisterUser inpUser, out int code)
        {
            var DbUsers = DbContext.Users;
            bool existingUser = DbUsers.Where(u => u.email == inpUser.email).Any();

            if (!existingUser)
            {
                string regexPatternEmail = "^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$";
                if (!Regex.IsMatch(inpUser.email, regexPatternEmail))
                {
                    response2 = new ResponseWithoutData(400, "Please Enter Valid Email", false);
                    code = 400;
                    return response2;
                }
                string regexPatternPassword = "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$";
                if (!Regex.IsMatch(inpUser.password, regexPatternPassword))
                {
                    response2 = new ResponseWithoutData(400, "Please Enter Valid Password. Must contain atleast one uppercase letter, one lowercase letter, one number and one special chararcter and must be atleast 8 characters long", false);
                    code = 400;
                    return response2;
                }
                string regexPatternPhone = "^[6-9]\\d{9}$";
                if (!Regex.IsMatch(inpUser.contactno.ToString(), regexPatternPhone))
                {
                    response2 = new ResponseWithoutData(400, "Please Enter Valid PhoneNo", false);
                    code = 400;
                    return response2;
                }


                var user = new User(Guid.NewGuid(), inpUser.firstName, inpUser.lastName, inpUser.email, inpUser.contactno, inpUser.address, _secondaryAuthService.CreatePasswordHash(inpUser.password), "pathToPic", "manager", "token");

                DbContext.Users.Add(user);
                DbContext.SaveChanges();

                //response object
                RegistrationLoginResponse data = new RegistrationLoginResponse(user.userId, user.email, user.firstName, user.lastName, user.userRole, "");
                response = new Response(200, "Manager added Successfully", data, true);
                _logger.LogInformation("Manager added successfully", data);
                code = 200;
                return response;
            }
            else
            {
                User userFound = DbUsers.Where(u => u.email == inpUser.email).First();
                if (userFound.isDeleted == true)
                {
                    response2 = new ResponseWithoutData(401, "Account with this email is restricted/deleted", false);
                    code = 401;
                    return response2;
                }
                response2 = new ResponseWithoutData(400, "Email already registered please try another", false);
                code = 400;
                return response2;
            }
        }
    }
}
