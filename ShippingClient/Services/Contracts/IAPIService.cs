using ShippingClient.Models;

namespace ShippingClient.Services.Contracts
{
    public interface IAPIService
    {
        public Task<GetProductsResponse> GetProductTypes(string? search = null);
        public Task<GetContainerTypesResponse> GetContainerTypes(string? search = null);
        public Task<GetCheckpointsResponse> GetCheckpoints();
        public Task<CreateShipmentResponse> CreateShipment(CreateShipment model);
        public Task<AddProductTypeResponse> AddProductType(AddProductType model);
        public Task<AddContainerTypeResponse> AddContainerType(AddContainerType model);
        public Task<AddDriverResponse> AddDriver(AddDriver model);
    }
}
