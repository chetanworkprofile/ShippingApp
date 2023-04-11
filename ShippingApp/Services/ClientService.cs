using ShippingApp.Controllers;
using ShippingApp.Data;
using ShippingApp.Models.OutputModels;
using ShippingApp.Models;
using ShippingApp.Models.InputModels;
using ShippingApp.RabbitMQ;

namespace ShippingApp.Services
{
    public class ClientService : IClientService
    {
        Response response = new Response();                             //response models/objects
        ResponseWithoutData response2 = new ResponseWithoutData();             // model to create token
        object result = new object();
        private readonly ShippingDbContext DbContext;
        private readonly ILogger<ClientController> _logger;
        private readonly IMessageProducer _messagePublisher;

        //constructor
        public ClientService(ShippingDbContext dbContext, ILogger<ClientController> logger, IMessageProducer messagePublisher)
        {
            DbContext = dbContext;
            _logger = logger;
            _messagePublisher = messagePublisher;
        }

        public Response CreateShipment(string userId,AddShipment inpShipment, out int code)
        {
            Guid userGuid = new Guid(userId);
            Shipment shipment = new Shipment(userGuid,inpShipment);
            _messagePublisher.SendShipment(shipment);
            response = new Response(200, "Your Shipment has been queued successfully", shipment, true);
            code = 200;
            return response;
        }
    }
}
