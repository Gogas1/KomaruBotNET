using KomaruBotASPNET.Abstractions;

namespace KomaruBotASPNET.Services
{
    public class PollingService(IServiceProvider serviceProvider) : PollingServiceBase<ReceiverService>(serviceProvider, serviceProvider.GetRequiredService<ILogger<PollingServiceBase<ReceiverService>>>())
    {
    }
}
