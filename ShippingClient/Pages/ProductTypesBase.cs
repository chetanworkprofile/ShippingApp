using Microsoft.AspNetCore.Components;
using ShippingClient.Models;
using ShippingClient.Services;
using ShippingClient.Services.Contracts;

namespace ShippingClient.Pages
{
    public class ProductTypesBase : ComponentBase
    {
        [Inject]
        public IProductTypeService ProductTypeService { get; set; }
        public IEnumerable<ProductType> productTypes { get; set; }


        protected override async Task OnInitializedAsync()
        {
            productTypes = await ProductTypeService.GetItems();
            //return base.OnInitializedAsync();
        }
    }
}
