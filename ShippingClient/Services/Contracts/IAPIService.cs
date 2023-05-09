using ShippingClient.Models;


namespace ShippingClient.Services.Contracts
{
    public interface IAPIService
    {
        public Task<GlobalResponse> GetProductTypes(string? search = null, Guid? productTypeId = null);
        public Task<GlobalResponse> AddCheckpoint(AddCheckpoint model);
        public Task<GlobalResponse> GetContainerTypes(string? search = null, Guid? containerTypeId = null);
        public Task<GlobalResponse> GetCheckpoints();
        public Task<GlobalResponse> GetCheckpointsByName(string name);
        public Task<GlobalResponse> CreateShipment(CreateShipment model);
        public Task<GlobalResponse> AddProductType(AddProductType model);
        public Task<GlobalResponse> GetCost(CreateShipment model);
        public Task<GlobalResponse> AddContainerType(AddContainerType model);
        public Task<GlobalResponse> AddDriver(AddDriver model);
        //public Task<LoginResponse> AddManager(AddManager model);
        public Task<GetUsersResponse> GetUsers(int pageNumber = 1, string? search = null);
        public Task<GlobalResponse> UpdateDriverLocation(Guid checkpointId);
        public Task<GetYourselfResponse> GetYourself();
        public Task<GetShipmentsCutomerResponse> GetShipmentsForCustomer(Guid customerId);
        public Task<ProductType> GetProductTypeSingle(Guid id);
        public Task<ContainerType> GetContainerTypeSingle(Guid id);
        public Task<Checkpoints> GetCheckpointTypeSingle(Guid id);
        public Task<GlobalResponse> GetShipmentHistory(Guid shipmentId);
        public Task<GlobalResponse> GetShipmentHistoryDriver();
        public Task<List<Checkpoints>> GetShortRoute(Guid shipmentId);
        public Task<List<DriverId>> GetDrivers(Guid driverId);
        public Task<List<AvailableShipmentsDriver>> GetAvailableShipments(Guid checkpointId);
        public Task<GlobalResponse> UpdateProductTypes(UpdateProductType model);
        public Task<GlobalResponse> UpdateContainerTypes(UpdateContainerType model);
        public Task<GlobalResponse> RemoveProductType(Guid productTypeId);
        public Task<GlobalResponse> RemoveContainerType(Guid containerTypeId);
        public Task<GlobalResponse> AcceptShipment(AcceptShipment model);
        public Task<GlobalResponse> UpdateUser(UpdateUser model);
        public Task<GlobalResponse> DeleteUser(DeleteUser model);
        public Task<GlobalResponse> GetDriverEarnings(Guid driverId);
        public Task<GlobalResponse> GetDriverEarningsForChart(Guid driverId);
        public Task<GlobalResponse> GetAdminEarnings();
        public Task<GlobalResponse> GetAdminEarningsForChart();
        public Task<GlobalResponse> CreateOrder(int amount);
        public Task<GlobalResponse> VerifyPayment(string paymentId, string orderId, string signature);
    }
}
