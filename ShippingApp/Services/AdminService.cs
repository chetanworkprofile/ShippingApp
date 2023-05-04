using ShippingApp.Controllers;
using ShippingApp.Data;
using ShippingApp.Models.OutputModels;
using ShippingApp.Models;
using ShippingApp.RabbitMQ;
using ShippingApp.Models.InputModels;
using System.Text.RegularExpressions;
using System.Configuration;
using Microsoft.EntityFrameworkCore;

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

        public Object GetUsers(string userId, string userType, string token, Guid? UserId, string? searchString, string? Email, long contactNo, string OrderBy, int SortOrder, int RecordsPerPage, int PageNumber, out int code)          // sort order   ===   e1 for ascending   -1 for descending
        {
            Guid id = new Guid(userId);
            var userLoggedIn = DbContext.Users.Find(id);
            var userss = DbContext.Users.AsQueryable();

            userss = userss.Where(t => t.isDeleted == false);     //remove deleted users from list

            if (userType != "all")
            {
                userss = userss.Where(t => t.userRole == userType);
            }

            if (token != userLoggedIn.token)
            {
                response2 = new ResponseWithoutData(401, "Invalid/expired token. Login First", false);
                code = 401;
                return response2;
            }

            //--------------------------filtering based on userId,searchString, Email, or Phone---------------------------------//

            if (UserId != null) { userss = userss.Where(s => (s.userId == UserId)); }
            if (searchString != null) { userss = userss.Where(s => EF.Functions.Like(s.firstName, "%" + searchString + "%") || EF.Functions.Like(s.lastName, "%" + searchString + "%") || EF.Functions.Like(s.firstName + " " + s.lastName, "%" + searchString + "%")); }
            //if (FirstName != null) { users = users.Where(s => (s.FirstName == FirstName)).ToList(); }
            if (Email != null) { userss = userss.Where(s => (s.email == Email)); }
            if (contactNo != -1) { userss = userss.Where(s => (s.contactNo == contactNo)); }

            var users = userss.ToList();
            if (!users.Any())
            {
                response2 = new ResponseWithoutData(404, "No User found.", true);
                code = 404;
                return response2;
            }
            // delegate used to create orderby depending on user input
            Func<User, Object> orderBy = s => s.userId;
            if (OrderBy == "UserId" || OrderBy == "ID" || OrderBy == "Id")
            {
                orderBy = x => x.userId;
            }
            else if (OrderBy == "FirstName" || OrderBy == "Name" || OrderBy == "firstname" || OrderBy == "firstName")
            {
                orderBy = x => x.firstName;
            }
            else if (OrderBy == "LastName" || OrderBy == "lastname" || OrderBy == "lastName")
            {
                orderBy = x => x.lastName;
            }
            else if (OrderBy == "Email" || OrderBy == "email")
            {
                orderBy = x => x.email;
            }
            else if (OrderBy == "UserRole" || OrderBy == "userRole" || OrderBy == "userrole")
            {
                orderBy = x => x.userRole;
            }

            // sort according to input based on orderby
            if (SortOrder == 1)
            {
                users = users.OrderBy(orderBy).Select(c => (c)).ToList();
            }
            else
            {
                users = users.OrderByDescending(orderBy).Select(c => (c)).ToList();
            }
            int count = users.Count;
            //pagination
            users = users.Skip((PageNumber - 1) * RecordsPerPage)
                                  .Take(RecordsPerPage).ToList();

            List<ResponseUser> list = new List<ResponseUser>();

            foreach (var user in users)
            {
                ResponseUser r = new ResponseUser(user);
                list.Add(r);
            }

            DataListForGet res = new DataListForGet(count, list);
            response = new Response(200, "Users list fetched", res, true);
            code = 200;
            return response;
        }

        public Object DeleteUser(string adminId, string userId, string token, out int code)
        {
            Guid id = new Guid(userId);
            Guid adminGuid = new Guid(adminId);
            User? user = DbContext.Users.Find(id);
            User admin = DbContext.Users.Where(s => (s.userId == adminGuid)).First();
            if (token != admin.token)
            {
                response2 = new ResponseWithoutData(401, "Invalid/expired token. Login First", false);
                code = 401;
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



/*public Object CreateManager(RegisterUser inpUser, out int code)
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
       }*/
