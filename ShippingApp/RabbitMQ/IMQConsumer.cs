using ShippingApp.Models.InputModels;

namespace ShippingApp.RabbitMQ
{
    public interface IMQConsumer
    {
        public void NotifyDeliveryBoy();
    }
}
