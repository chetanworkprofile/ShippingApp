using ShippingApp.Models.OutputModels;

namespace ShippingApp.Services
{
    
    public interface IAPIGatewayService
    {
        public string GetShipments(Guid? shipmentId, Guid? customerId, Guid? productTypeId, Guid? containerTypeId);
        public string GetProductTypes(Guid? productTypeId, string? searchString);
    }
}
