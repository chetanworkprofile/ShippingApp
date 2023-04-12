using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace ShippingApp.RabbitMQ
{
    public class MQConsumer : IMQConsumer
    {
        private ConnectionFactory factory;
        public MQConsumer()
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
        public void ShipmentConsumer()
        {
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.QueueDeclare("createShipment",
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(message);
                /*AddShipmentModel shipment = JsonSerializer.Deserialize<>(message)!;
                if (shipment != null)
                {
                    var response = shipmentService!.AddShipment(shipment!);
                }*/
            };
            channel.BasicConsume("createShipment", true, consumer);
            Console.ReadLine();
        }
    }
}
