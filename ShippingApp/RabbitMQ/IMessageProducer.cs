using ShippingApp.Models.OutputModels;

namespace ShippingApp.RabbitMQ
{
    public interface IMessageProducer
    {
        public void SendShipment<T>(T message);
        public ResponseWithoutData SendEmail<T>(T message);
    }
}
//producer interface