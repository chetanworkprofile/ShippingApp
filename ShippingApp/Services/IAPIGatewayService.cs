using ShippingApp.Models;
using ShippingApp.Models.InputModels;
using ShippingApp.Models.OutputModels;

namespace ShippingApp.Services
{
    
    public interface IAPIGatewayService
    {
        public string GetShipments(Guid? shipmentId, Guid? customerId, Guid? productTypeId, Guid? containerTypeId, out int code);
        public string GetShipmentHistory(Guid? shipmentId, out int code);
        public string GetAvailableShipments(Guid checkpointId, out int code);
        public string GetShipmentHistoryDriver(string driverId, out int code);
        public string GetBestRoute(Guid? shipmentId, out int code);
        public string GetProductTypes(Guid? productTypeId, string? searchString, out int code);
        public string GetContainerTypes(Guid? containerTypeId, string? searchString, out int code);
        public string GetCost(AddShipment inp, out int code);
        public string AddProductType(AddProductType inp, out int code);
        public string AddContainerType(AddContainerType inp, out int code);
        public string AddCheckpoint(AddCheckpoint inp, out int code);
        public object AddDriver(RegisterDriver inpUser, out int code);
        public Response GetDrivers(Guid? driverId, string? searchString, string? location, out int code);
        public string GetCheckpoints(Guid? checkpointId,string? checkpointName, out int code);
        public object UpdateDriverLocation(string userId, string token, Guid checkPointId, out int code);
        public string AcceptShipment(AcceptShipment inp, out int code);
        public object UpdateProductType(string userId, string token, UpdateProductType model, out int code);
        public object UpdateContainerType(string userId, string token, UpdateContainerType model, out int code);
    }
}
