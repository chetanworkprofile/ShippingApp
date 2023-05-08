using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using RabbitMQ.Client;
using ShippingApp.Models.OutputModels;
using System.Text;
using System.Threading.Channels;

namespace ShippingApp.RabbitMQ
{
    public class MQProducer : IMessageProducer
    {
        private ConnectionFactory factory;
        private readonly IConfiguration _configuration; //get configuration values from appsettings
        public MQProducer(IConfiguration configuration)
        {
            _configuration = configuration;
            factory = new ConnectionFactory                 // create connection factory
            {
                HostName = _configuration.GetSection("RabbitMq:url").Value!,
                Port = Protocols.DefaultProtocol.DefaultPort,
                UserName = _configuration.GetSection("RabbitMq:user").Value!,
                Password = _configuration.GetSection("RabbitMq:password").Value!,
                VirtualHost = "/",
                ContinuationTimeout = new TimeSpan(10, 0, 0, 0)
            };
            
        }
        public void SendShipment<T>(T message)
        {
            var connection = factory.CreateConnection();        //create connection through factory
            using var channel = connection.CreateModel();
            channel.QueueDeclare("createShipment",      // declare queue
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

            var json = JsonConvert.SerializeObject(message);        //serialize object
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: "", routingKey: "createShipment", body: body);       //publish the produced item to queue
        }
        public ResponseWithoutData SendEmail<T>(T message)          //function used to send email through microservices it sends the content and email address to queue
        {
            var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare("sendEmail",
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: "", routingKey: "sendEmail", body: body);
            return new ResponseWithoutData(200, "verification Email Sent Successfully", true);
        }
    }
}
