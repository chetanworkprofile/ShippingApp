﻿using ShippingClient.Models;

namespace ShippingClient.Services.Contracts
{
    public interface IAPIService
    {
        public Task<GlobalResponse> GetProductTypes(string? search = null, Guid? productTypeId = null);
        public Task<ResponseModel> AddCheckpoint(AddCheckpoint model);
        public Task<GlobalResponse> GetContainerTypes(string? search = null, Guid? containerTypeId = null);
        public Task<GetCheckpointsResponse> GetCheckpoints();
        public Task<GlobalResponse> GetCheckpointsByName(string name);
        public Task<CreateShipmentResponse> CreateShipment(CreateShipment model);
        public Task<AddProductTypeResponse> AddProductType(AddProductType model);
        public Task<GlobalResponse> GetCost(CreateShipment model);
        public Task<AddContainerTypeResponse> AddContainerType(AddContainerType model);
        public Task<AddDriverResponse> AddDriver(AddDriver model);
        //public Task<LoginResponse> AddManager(AddManager model);
        public Task<GetUsersResponse> GetUsers(int pageNumber = 1, string? search = null);
        public Task<UpdateDriverLocationResponse> UpdateDriverLocation(Guid checkpointId);
        public Task<GetYourselfResponse> GetYourself();
        public Task<GetShipmentsCutomerResponse> GetShipmentsForCustomer(Guid customerId);
        public Task<ProductType> GetProductTypeSingle(Guid id);
        public Task<ContainerType> GetContainerTypeSingle(Guid id);
        public Task<Checkpoints> GetCheckpointTypeSingle(Guid id);
        public Task<ShipmentHistory> GetShipmentHistory(Guid shipmentId);
        public Task<List<AvailableShipmentsDriver>> GetShipmentHistoryDriver();
        public Task<List<CheckpointModel>> GetShortRoute(Guid shipmentId);
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

	}
}
