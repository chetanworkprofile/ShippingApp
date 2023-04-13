using ShippingClient.Models;

namespace ShippingClient.Services.Contracts
{
    public interface IProductTypeService
    {
        public Task<IEnumerable<ProductType>> GetItems();
    }
}
