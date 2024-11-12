
namespace KomaruBotASPNET.Abstractions
{
    public abstract class PollingServiceBase<TReceiverService> : BackgroundService 
        where TReceiverService : IReceiverService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<PollingServiceBase<TReceiverService>> _logger;

        public PollingServiceBase(IServiceProvider serviceProvider, ILogger<PollingServiceBase<TReceiverService>> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await DoWork(stoppingToken);
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var receiver = scope.ServiceProvider.GetRequiredService<TReceiverService>();

                    await receiver.ReceiveAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
            }
        }
    }
}
