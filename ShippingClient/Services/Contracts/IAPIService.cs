using ShippingClient.Models;

namespace ShippingClient.Services.Contracts
{
    public interface IAPIService
    {
        public Task<GetProductsResponse> GetProductTypes();
        public Task<GetContainerTypesResponse> GetContainerTypes();
        public Task<GetCheckpointsResponse> GetCheckpoints();
    }
}
