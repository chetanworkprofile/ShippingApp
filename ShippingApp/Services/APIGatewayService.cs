using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using ShippingApp.Models;
using System.Text;
using ShippingApp.Controllers;
using ShippingApp.Data;
using ShippingApp.Models.OutputModels;
using ShippingApp.RabbitMQ;

namespace ShippingApp.Services
{
    public class APIGatewayService : IAPIGatewayService
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private string baseUrlS1;
        private string baseUrlS2;

        Response response = new Response();                             //response models/objects
        ResponseWithoutData response2 = new ResponseWithoutData();             // model to create token
        object result = new object();
        private readonly ShippingDbContext DbContext;
        private readonly ILogger<APIGatewayController> _logger;
        private readonly IMessageProducer _messagePublisher;
        public APIGatewayService(ShippingDbContext dbContext, ILogger<APIGatewayController> logger, IMessageProducer messagePublisher)
        {
            DbContext = dbContext;
            _logger = logger;
            _messagePublisher = messagePublisher;
            baseUrlS1 = "http://192.180.2.128:4000";
            baseUrlS2 = "http://192.180.0.127:4040";
        }

        public string GetShipments(Guid? shipmentId, Guid? customerId, Guid? productTypeId, Guid? containerTypeId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrlS1);//WebApi 1 project URL
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                StringBuilder appendUrl = new StringBuilder("api/Shipment?");
                if (shipmentId != null)
                {
                    appendUrl.Append("&shipmentId=" + shipmentId + "");

                }
                if (customerId != null)
                {
                    appendUrl.Append("&customerId=" + customerId + "");
                }
                if (productTypeId != null)
                {
                    appendUrl.Append("&productTypeId=" + productTypeId + "");
                }
                if (containerTypeId != null)
                {
                    appendUrl.Append("&containerTypeId=" + containerTypeId + "");
                }
                var res = client.GetAsync(appendUrl.ToString()).Result;

                var data = res.Content.ReadAsStringAsync().Result;

                //response = new Response(200, "Shipments list fetched", data, true);
                return data;
            }
        }


        public string GetProductTypes(Guid? productTypeId, string? searchString)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrlS2);//WebApi 1 project URL
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                StringBuilder appendUrl = new StringBuilder("api/ProductType/Search?");
                if (productTypeId != null)
                {
                    appendUrl.Append("&productTypeId=" + productTypeId + "");

                }
                if (searchString != null)
                {
                    appendUrl.Append("&searchString=" + searchString + "");
                }
                
                var res = client.GetAsync(appendUrl.ToString()).Result;

                var data = res.Content.ReadAsStringAsync().Result;

                //response = new Response(200, "Shipments list fetched", data, true);
                return data;
            }
        }

    }
}