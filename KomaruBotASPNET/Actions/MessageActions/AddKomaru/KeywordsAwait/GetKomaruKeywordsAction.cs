using KomaruBotASPNET.Extensions;
using KomaruBotASPNET.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KomaruBotASPNET.Actions.MessageActions.AddKomaru.KeywordsAwait
{
    public class GetKomaruKeywordsAction : ResultAction<Message>
    {
        private readonly ITelegramBotClient telegramBotClient;
        private readonly UserService userService;

        private const string FailureText = "Необходим текст";
        private const string SuccessText = "Слово принято";

        public GetKomaruKeywordsAction(ITelegramBotClient telegramBotClient, UserService userService)
        {
            this.telegramBotClient = telegramBotClient;
            this.userService = userService;
        }

        public override async Task<Message?> Execute(Message msg)
        {
            if (!msg.ValidateMessage(true))
            {
                return await telegramBotClient.SendTextMessageAsync(msg.Chat.Id, FailureText);
            }

            var keywords = msg.Text!.Trim().Split(' ');

            await userService.SetUserStateInputStateAsync(msg.From!.Id, us =>
            {
                var previousKeywords = new List<string>(us.AddKomaruFlow.Keywords);
                previousKeywords.AddRange(keywords);

                us.SetAddKomaruFlow(new Models.StateFlows.AddKomaruFlow
                {
                    FileId = us.AddKomaruFlow.FileId,
                    Name = us.AddKomaruFlow.Name,
                    Keywords = previousKeywords,
                    FileType = us.AddKomaruFlow.FileType,
                });
            });

            return await telegramBotClient.SendTextMessageAsync(msg.Chat.Id, SuccessText);
        }
    }
}
