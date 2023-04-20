using ShippingClient.Models;

namespace ShippingClient.Services.Contracts
{
    public interface IAPIService
    {
        public Task<GetProductsResponse> GetProductTypes(string? search = null);
        public Task<ResponseModel> AddCheckpoint(AddCheckpoint model);
        public Task<GetContainerTypesResponse> GetContainerTypes(string? search = null);
        public Task<GetCheckpointsResponse> GetCheckpoints();
        public Task<CreateShipmentResponse> CreateShipment(CreateShipment model);
        public Task<AddProductTypeResponse> AddProductType(AddProductType model);
        public Task<AddContainerTypeResponse> AddContainerType(AddContainerType model);
        public Task<AddDriverResponse> AddDriver(AddDriver model);
        public Task<LoginResponse> AddManager(AddManager model);
        public Task<GetUsersResponse> GetUsers(int pageNumber = 1, string? search = null);
        public Task<UpdateDriverLocationResponse> UpdateDriverLocation(Guid checkpointId);
        public Task<GetYourselfResponse> GetYourself();
        public Task<GetShipmentsCutomerResponse> GetShipmentsForCustomer(Guid customerId);
        public Task<ProductType> GetProductTypeSingle(Guid id);
        public Task<ContainerType> GetContainerTypeSingle(Guid id);
        public Task<Checkpoints> GetCheckpointTypeSingle(Guid id);
        public Task<ShipmentHistory> GetShipmentHistory(Guid shipmentId);
        public Task<GetShipmentsCutomerResponse> GetShipmentHistoryDriver();
        public Task<List<CheckpointModel>> GetShortRoute(Guid shipmentId);
    }
}
