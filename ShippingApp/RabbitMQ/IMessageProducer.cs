namespace ShippingApp.RabbitMQ
{
    public interface IMessageProducer
    {
        public void SendMessage<T>(T message);
    }
}
