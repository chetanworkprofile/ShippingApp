using ShippingClient.Models;
using ShippingClient.Services.Contracts;
using System.Net.Http.Json;

namespace ShippingClient.Services
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly HttpClient httpClient;
        public ProductTypeService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<ProductType>> GetItems()
        {
            try
            {
                var productTypes = await this.httpClient.GetFromJsonAsync<ResponseProductTypes>("api/v1/get/productTypes");
                var productTypeList = productTypes.data.ToList();
                return productTypeList!;
            }
            catch ( Exception ex) 
            {
                //log exception
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
