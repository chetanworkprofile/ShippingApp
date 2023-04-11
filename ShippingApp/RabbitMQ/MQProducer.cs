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
        public MQProducer()
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
            
        }
        public void SendShipment<T>(T message)
        {
            var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare("createShipment", 
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: "", routingKey: "createShipment", body: body);
        }
        public ResponseWithoutData SendEmail<T>(T message)
        {
            var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare("sendEmail",
                     durable: false,
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
