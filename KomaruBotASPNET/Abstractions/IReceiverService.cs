namespace KomaruBotASPNET.Abstractions
{
    public interface IReceiverService
    {
        Task ReceiveAsync(CancellationToken stoppingToken);
    }
}
