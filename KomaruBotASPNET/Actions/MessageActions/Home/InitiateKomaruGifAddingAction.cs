using KomaruBotASPNET.Extensions;
using KomaruBotASPNET.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace KomaruBotASPNET.Actions.MessageActions.Home
{
    public class InitiateKomaruGifAddingAction : ResultAction<Message>
    {
        private readonly AuthorizationService authorizationService;
        private readonly UserService userService;
        private readonly ITelegramBotClient telegramBotClient;

        private const string ActionText = """
            <b>Добавление кумара:</b>
            Три шага:
            1: Отправка файла;
            2: Название для удобного поиска;
            3: Ключевые слова для удобного поиска;
            Сейчас первый шаг - жду файл.
            """;

        public InitiateKomaruGifAddingAction(AuthorizationService authorizationService, GifService gifService, UserService userService, ITelegramBotClient telegramBotClient)
        {
            this.authorizationService = authorizationService;
            this.userService = userService;
            this.telegramBotClient = telegramBotClient;
        }

        public override async Task<Message?> Execute(Message msg)
        {
            if (msg.From == null)
            {
                return null;
            }

            if (!authorizationService.IsAdminAccount(msg.From.Id))
            {
                return null;
            }

            await userService.SetUserStateAsync(Enums.UserState.KomaruFileAwait, msg.From.Id);
            await userService.SetUserStateInputStateAsync(msg.From.Id, us =>
            {
                us.SetAddKomaruFlow(new Models.StateFlows.AddKomaruFlow());
            });

            return await telegramBotClient.SendTextMessageAsync(msg.Chat, ActionText, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: new ReplyKeyboardRemove());
        }
    }
}
