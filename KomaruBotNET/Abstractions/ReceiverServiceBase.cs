using Telegram.Bot;
using Telegram.Bot.Polling;

namespace KomaruBotASPNET.Abstractions
{
    public class ReceiverServiceBase<TUpdateHandler> : IReceiverService 
        where TUpdateHandler : IUpdateHandler
    {

        private readonly ITelegramBotClient _botClient;
        private readonly TUpdateHandler _updateHandler;

        public ReceiverServiceBase(ITelegramBotClient botClient, TUpdateHandler updateHandler)
        {
            _botClient = botClient;
            _updateHandler = updateHandler;
        }

        public async Task ReceiveAsync(CancellationToken stoppingToken)
        {
            var receiverOptions = new ReceiverOptions()
            {
                AllowedUpdates = [],
                DropPendingUpdates = true
            };

            await _botClient.ReceiveAsync(updateHandler: _updateHandler, receiverOptions: receiverOptions, cancellationToken: stoppingToken);
        }
    }
}
