using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace ShippingApp.RabbitMQ
{
    public class MQProducer : IMessageProducer
    {
        public void SendMessage<T>(T message)
        {
            var factory = new ConnectionFactory
            {
                HostName = "192.180.3.63",
                Port = Protocols.DefaultProtocol.DefaultPort,
                UserName = "s1",
                Password = "guest",
                VirtualHost = "/",
                ContinuationTimeout = new TimeSpan(10, 0, 0, 0)
            };
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
    }
}
