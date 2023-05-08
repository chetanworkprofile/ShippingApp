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
        private ConnectionFactory factory;          //rabbitmq var to create connection
        private IHubContext<ShippingHub> _hubContext;   //hub instance
        private IMessageProducer _producer;     //producer instance
        private ShippingDbContext _shippingDbContext;   //dbcontext instance
        private readonly IConfiguration _configuration;        //used to get configuration values from appsettings
        public MQConsumer(ShippingDbContext dbContext, ILogger<string> logger, IHubContext<ShippingHub> hubContext, IMessageProducer producer, IConfiguration configuration)
        {
            _configuration= configuration;
            factory = new ConnectionFactory
            {
                HostName = _configuration.GetSection("RabbitMq:url").Value!,
                Port = Protocols.DefaultProtocol.DefaultPort,
                UserName = _configuration.GetSection("RabbitMq:user").Value!,
                Password = _configuration.GetSection("RabbitMq:password").Value!,
                VirtualHost = "/",
                ContinuationTimeout = new TimeSpan(10, 0, 0, 0)
            };
            _shippingDbContext= dbContext;
            _hubContext = hubContext;
            _producer = producer;
        }
        public void NotifyDeliveryBoy()     //function used to notify delivery persons of new shipment in thier area
        {
            var connection = factory.CreateConnection();    //create connection
            var channel = connection.CreateModel(); 
            channel.QueueDeclare("notifyDriver",        //declare queue 
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
            var consumer = new EventingBasicConsumer(channel);      //create consumer
            consumer.Received += async (sender, e) =>               //on receving new msg in queue
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                //Console.WriteLine(message);
                NotifyDriver driver = JsonSerializer.Deserialize<NotifyDriver>(message)!;       //get list of driver ids whom to notify
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

    public static class ConnectionIds       //dictionary of userids and connection ids that are currently connected i.e, online
    {
        public static readonly Dictionary<string, string> Users = new Dictionary<string, string>();
    }
}
