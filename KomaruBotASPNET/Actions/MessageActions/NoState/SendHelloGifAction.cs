using KomaruBotASPNET.Models;
using KomaruBotASPNET.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KomaruBotASPNET.Actions.MessageActions.NoState
{
    public class SendHelloGifAction : ResultAction<Message>
    {
        private readonly GifService _gifService;
        private readonly ITelegramBotClient _botClient;

        public SendHelloGifAction(GifService gifService, ITelegramBotClient botClient)
        {
            _gifService = gifService;
            _botClient = botClient;
        }

        public override async Task<Message?> Execute(Message msg)
        {
            KomaruGif? targetGif = await _gifService.GetGifByKeyword("привет");

            if (targetGif == null)
            {
                return await _botClient.SendTextMessageAsync(msg.Chat, "У меня нет подходящей приветственной гифки(");
            }

            InputFile inputFile = InputFile.FromFileId(targetGif.TelegramId);

            return await _botClient.SendPhotoAsync(msg.Chat, inputFile);
        }
    }
}
