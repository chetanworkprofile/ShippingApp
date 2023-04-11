using ShippingApp.Models.InputModels;
using ShippingApp.Models.OutputModels;

namespace ShippingApp.Services
{
    public interface IClientService
    {
        public Response CreateShipment(string userId, AddShipment inpShipment, out int code);
    }
}
