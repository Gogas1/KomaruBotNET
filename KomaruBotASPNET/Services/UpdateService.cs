using KomaruBotASPNET.DbContexts;
using KomaruBotASPNET.Enums;
using KomaruBotASPNET.States;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace KomaruBotASPNET.Services
{
    public class UpdateService : IUpdateHandler
    {
        private readonly UserService _userService;
        private readonly StateHandlersFactory<Message> _stateHandlersFactory;
        private readonly ILogger _logger;

        public UpdateService(StateHandlersFactory<Message> stateHandlersFactory, ILogger<UpdateService> logger, UserService userService)
        {
            _stateHandlersFactory = stateHandlersFactory;
            _logger = logger;
            _userService = userService;
        }

        public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
        {
            _logger.LogError(exception.Message);
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await (update switch
            {
                { Message: { } message } => OnMessage(message),
                _ => UnknownUpdateHandlerAsync(update)
            });
        }

        private async Task OnMessage(Message msg)
        {
            if (msg.From == null)
            {
                return;
            }

            UserState userState = await _userService.GetUserStateByTelegramIdAsync(msg.From.Id);

            StateHandlerBase<Message>? targetStateHandler = _stateHandlersFactory.GetStateHandler(userState);
            
            if(targetStateHandler == null)
            {
                return;
            }

            await targetStateHandler.Handle(msg);
        }

        private Task UnknownUpdateHandlerAsync(Update update)
        {        
            return Task.CompletedTask;
        }
    }
}
