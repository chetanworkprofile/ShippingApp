using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using ShippingApp.Models.InputModels;
using Microsoft.AspNetCore.SignalR;
using ShippingApp.Hubs;
using ShippingApp.Data;
using ShippingApp.Models;

namespace ShippingApp.RabbitMQ
{
    public class MQConsumer : IMQConsumer
    {
        private ConnectionFactory factory;
        private IHubContext<ShippingHub> _hubContext;
        private IMessageProducer _producer;
        private ShippingDbContext _shippingDbContext;
        public MQConsumer(ShippingDbContext dbContext, ILogger<string> logger, IHubContext<ShippingHub> hubContext, IMessageProducer producer)
        {
            factory = new ConnectionFactory
            {
                HostName = "192.180.3.63",
                Port = Protocols.DefaultProtocol.DefaultPort,
                UserName = "s1",
                Password = "guest",
                VirtualHost = "/",
                ContinuationTimeout = new TimeSpan(10, 0, 0, 0)
            };
            _shippingDbContext= dbContext;
            _hubContext = hubContext;
            _producer = producer;
        }
        public void NotifyDeliveryBoy()
        {
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.QueueDeclare("notifyDriver",
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(message);
                NotifyDriver driver = JsonSerializer.Deserialize<NotifyDriver>(message)!;
                if (driver != null)
                {
                    foreach(var id in driver.driverIds)
                    {
                        EmailDriver(id);
                        //var response = shipmentService!.AddShipment(shipment!);
                        string driverConnId = GetConnectionIdByUser(id.ToString());
                        Console.WriteLine(driver);
                        Console.WriteLine(driverConnId);
                        await _hubContext.Clients.Clients(driverConnId).SendAsync("refresh");
                    }
                    //var res = _hubContext.SendShipmentForDelivery(driver.shipmentId.ToString(), driver.driverId.ToString());
                }
            };
            
            channel.BasicConsume("notifyDriver", true, consumer);
            Console.ReadLine();
        }

        public void EmailDriver(Guid driverId)
        {
            User driver = _shippingDbContext.Users.Find(driverId);
            SendEmailModel model = new SendEmailModel(driver.email,"New Shipment ",$"New Shipment is listed in your location, please check out");
            _producer.SendEmail(model);
        }

        public string GetConnectionIdByUser(string user)
        {
            lock (ConnectionIds.Users)
            {
                return ConnectionIds.Users.Where(x => x.Key == user).Select(x => x.Value).FirstOrDefault();
            }
        }
    }

    public static class ConnectionIds
    {
        public static readonly Dictionary<string, string> Users = new Dictionary<string, string>();
    }
}
