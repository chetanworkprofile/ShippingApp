using ShippingApp.Data;
using ShippingApp.Models.OutputModels;

namespace ShippingApp.Services
{
    public class UploadPicService : IUploadPicService
    {
        Response response;
        ResponseWithoutData response2 = new ResponseWithoutData();
        private readonly ShippingDbContext DbContext;

        public UploadPicService(ShippingDbContext dbContext)
        {
            response = new Response();
            DbContext = dbContext;
        }

        public Object ProfilePicUpload(IFormFile file, string userId, string token, string userRole, out int code)
        {
            try
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "ProfilePics");
                string path1 = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "ProfilePics", "client");
                string path2 = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "ProfilePics", "deliveryBoy");
                string path3 = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "ProfilePics", "manager");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (!Directory.Exists(path1))
                {
                    Directory.CreateDirectory(path1);
                }
                if (!Directory.Exists(path2))
                {
                    Directory.CreateDirectory(path2);
                }
                if (!Directory.Exists(path3))
                {
                    Directory.CreateDirectory(path3);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Guid id = new Guid(userId);
            var user = DbContext.Users.Find(id);
            var folderName = Path.Combine("Assets", "ProfilePics", userRole);
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            if (token != user.token)
            {
                response2 = new ResponseWithoutData(401, "Invalid/expired token. Login First", false);
                code = 401;
                return response2;
            }
            if (file == null)
            {
                response2 = new ResponseWithoutData(400, "Please provide a file for successful upload", false);
                code = 400;
                return response2;
            }
            if (file.Length > 0)
            {
                var fileName = string.Concat(
                                    user.email,
                                    DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                                    Path.GetFileNameWithoutExtension(file.FileName),
                                    Path.GetExtension(file.FileName)
                                    );

                var fullPath = Path.Combine(pathToSave, fileName);

                using (var stream = File.Create(fullPath))
                {
                    file.CopyTo(stream);
                }
                /*user.pathToProfilePic = Path.Combine(folderName, fileName);
                DbContext.SaveChanges();*/

                ResponseUser responseUser = new ResponseUser(user);
                PicUploadResponse data = new PicUploadResponse()
                {
                    //User = responseUser,
                    fileName = fileName,
                    pathToPic = Path.Combine(folderName, fileName)
                };
                response = new Response(200, "File Uploaded Successfully", data, true);
                code = 200;
                return response;
            }
            response2 = new ResponseWithoutData(400, "Please provide a file for successful upload", false);
            code = 400;
            return response2;
        }
    }
}
