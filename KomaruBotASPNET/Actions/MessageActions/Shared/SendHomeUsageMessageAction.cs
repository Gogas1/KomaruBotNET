using KomaruBotASPNET.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;

namespace KomaruBotASPNET.Actions.MessageActions.Shared
{
    public class SendHomeUsageMessageAction : ResultAction<Message>
    {
        private readonly ITelegramBotClient _botClient;
        private readonly AuthorizationService _authorizationService;

        private const string UsageText = """
            <b>Использование:</b>
            Вызовите бота в любом чате через @MyKomaruBot и добавьте ваш запрос
            """;

        private const string ExtendedUsageText = """
            <b>Меню бота:</b>
            Вызовите бота в любом чате через @MyKomaruBot и добавьте ваш запрос

            <b>Админское:</b>
            /addkomaru - Добавить гифку с кумаром
            """;

        public SendHomeUsageMessageAction(ITelegramBotClient botClient, AuthorizationService authorizationService)
        {
            _botClient = botClient;
            _authorizationService = authorizationService;
        }

        public override async Task<Message?> Execute(Message msg)
        {
            if (_authorizationService.IsAdminAccount(msg.From?.Id ?? 0))
            {
                return await _botClient.SendTextMessageAsync(msg.Chat, ExtendedUsageText, parseMode: ParseMode.Html, replyMarkup: new ReplyKeyboardRemove());
            }

            return await _botClient.SendTextMessageAsync(msg.Chat, UsageText, parseMode: ParseMode.Html, replyMarkup: new ReplyKeyboardRemove());
        }

    }
}
