using ShippingApp.Models.InputModels;
using ShippingApp.Models.OutputModels;

namespace ShippingApp.Services
{
    
    public interface IAPIGatewayService
    {
        public string GetShipments(Guid? shipmentId, Guid? customerId, Guid? productTypeId, Guid? containerTypeId, out int code);
        public string GetProductTypes(Guid? productTypeId, string? searchString, out int code);
        public string GetContainerTypes(Guid? containerTypeId, string? searchString, out int code);
        public string AddProductType(AddProductType inp, out int code);
        public string AddContainerType(AddContainerType inp, out int code);
        public object AddDriver(RegisterDriver inpUser, out int code);
        public Response GetDrivers(Guid? driverId, string? searchString, string? location, out int code);
        public string GetCheckpoints(Guid? checkpointId, out int code);
        public object UpdateDriverLocation(string userId, string token, Guid checkPointId, out int code);
    }
}
