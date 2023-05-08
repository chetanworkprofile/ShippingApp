using RabbitMQ.Client.Events;
using RabbitMQ.Client;

namespace ShippingApp.RabbitMQ
{
    public class BackgroundServiceConsumer : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;        //scope factory is used to add various scopes services like mqconsumer in background service
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();  //token used to cancel background service

        public BackgroundServiceConsumer(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        public Task StartAsync(CancellationToken cancellationToken)         // start background service
        {
            Task.Run(() => ConsumerQueues());       //on start consume rabbitmq queues and call function consumerqueues
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)    //stop service
        {
            _cancellationTokenSource.Cancel();
            await Task.CompletedTask;
        }
        public void ConsumerQueues()        // function used to start consumer function in MQConsumer to consume all queues
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var consumerService = scope.ServiceProvider.GetService<IMQConsumer>();      //create scope
                consumerService.NotifyDeliveryBoy();        //call function
            }
        }
    }
}

