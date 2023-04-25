using RabbitMQ.Client.Events;
using RabbitMQ.Client;

namespace ShippingApp.RabbitMQ
{
    public class BackgroundServiceConsumer : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public BackgroundServiceConsumer(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(() => ConsumerQueues());
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource.Cancel();
            await Task.CompletedTask;
        }
        public void ConsumerQueues()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var consumerService = scope.ServiceProvider.GetService<IMQConsumer>();
                consumerService.NotifyDeliveryBoy();
            }
        }
    }
}

