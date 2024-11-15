using KomaruBotASPNET.DbContexts;
using KomaruBotASPNET.Enums;
using KomaruBotASPNET.States.Abstractions;
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
        private readonly StateHandlersFactory<Message> _stateHandlersMessageFactory;
        private readonly StateHandlersFactory<InlineQuery> _stateHandlersInlineQueryFactory;
        private readonly ILogger _logger;

        public UpdateService(
            StateHandlersFactory<Message> stateHandlersFactory, 
            ILogger<UpdateService> logger, 
            UserService userService, 
            StateHandlersFactory<InlineQuery> stateHandlersInlineQueryFactory)
        {
            _stateHandlersMessageFactory = stateHandlersFactory;
            _logger = logger;
            _userService = userService;
            _stateHandlersInlineQueryFactory = stateHandlersInlineQueryFactory;
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
                { InlineQuery: { } inlineQuery } => OnInlineQuery(inlineQuery),
                _ => UnknownUpdateHandlerAsync(update)
            });
        }

        private async Task OnInlineQuery(InlineQuery inlineQuery)
        {
            if (inlineQuery.From == null)
            {
                return;
            }

            UserState userState = await _userService.GetUserStateByTelegramIdAsync(inlineQuery.From.Id);

            StateHandlerBase<InlineQuery>? targetStateHandler = _stateHandlersInlineQueryFactory.GetStateHandler(userState);

            if (targetStateHandler == null)
            {
                return;
            }

            await targetStateHandler.Handle(inlineQuery);
        }

        private async Task OnMessage(Message msg)
        {
            if (msg.From == null)
            {
                return;
            }

            UserState userState = await _userService.GetUserStateByTelegramIdAsync(msg.From.Id);

            StateHandlerBase<Message>? targetStateHandler = _stateHandlersMessageFactory.GetStateHandler(userState);
            
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
