using KomaruBotASPNET.Extensions;
using KomaruBotASPNET.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace KomaruBotASPNET.Actions.MessageActions.AddKomaru.NameAwait
{
    public class GetKomaruNameAction : ResultAction<Message>
    {
        private readonly UserService _userService;
        private readonly ITelegramBotClient _telegramBotClient;

        private const string FailureMessage = "Нужно отправить текст в качестве названия";
        private const string SuccessMessage = "Название принято. Теперь ключевые слова. Команда /end для прерывания ввода";

        public GetKomaruNameAction(UserService userService, ITelegramBotClient telegramBotClient)
        {
            _userService = userService;
            _telegramBotClient = telegramBotClient;
        }

        public override async Task<Message?> Execute(Message msg)
        {
            if (!msg.ValidateMessage(validateText: true))
            {
                return await _telegramBotClient.SendMessage(msg.Chat.Id, FailureMessage, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: new ReplyKeyboardRemove());
            }

            await _userService.SetUserStateInputStateAsync(msg.From!.Id, us =>
            {
                us.SetAddKomaruFlow(new Models.StateFlows.AddKomaruFlow
                {
                    FileId = us.AddKomaruFlow.FileId,
                    Name = msg.Text!,
                    FileType = us.AddKomaruFlow.FileType,
                });
            });

            await _userService.SetUserStateAsync(Enums.UserState.KomaruKeywordsAwait, msg.From.Id);

            return await _telegramBotClient.SendMessage(msg.Chat.Id, SuccessMessage, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: new ReplyKeyboardRemove());
        }
    }
}
