namespace ShippingApp.Services
{
    public interface IUploadPicService
    {
        public Object ProfilePicUpload(IFormFile file, string userId, string token, string userRole, out int code);
    }
}
