using KomaruBotASPNET.Enums;
using KomaruBotASPNET.Extensions;
using KomaruBotASPNET.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace KomaruBotASPNET.Actions.MessageActions.AddKomaru.FileAwait
{
    public class GetKomaruFileAction : ResultAction<Message>
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly UserService _userService;

        public GetKomaruFileAction(ITelegramBotClient telegramBotClient, UserService userService)
        {
            _telegramBotClient = telegramBotClient;
            _userService = userService;
        }

        private const string RejectMessageText = "На этом этапе нужно предоставить файл или стикер.";
        private const string AcceptMessageText = "Файл получен, теперь название";

        public override async Task<Message?> Execute(Message msg)
        {
            if (msg.From == null || msg.Photo == null && msg.Sticker == null && msg.Document == null)
            {
                return await SendRejectMessage(msg);
            }

            string? fileId = GetFileId(msg);
            if (fileId == null)
            {
                return await SendRejectMessage(msg);
            }

            await SaveState(fileId, msg.From.Id, GetFileType(msg));
            return await SendSuccessMessage(msg);
        }

        private string? GetFileId(Message msg)
        {
            if (msg.Photo != null && msg.Photo.Length > 0)
            {
                return msg.Photo[0].FileId;
            }

            if (msg.Sticker != null)
            {
                return msg.Sticker.FileId;
            }

            if (msg.Document != null)
            {
                return msg.Document.FileId;
            }

            return null;
        }

        private async Task SaveState(string fileId, long userId, FileType fileType)
        {
            await _userService.SetUserStateInputStateAsync(userId, us =>
            {
                us.SetAddKomaruFlow(new Models.StateFlows.AddKomaruFlow
                {
                    FileId = fileId,
                    FileType = fileType
                });
            });
            await _userService.SetUserStateAsync(UserState.KomaruNameAwait, userId);
        }

        private Task<Message> SendRejectMessage(Message msg) =>
            _telegramBotClient.SendTextMessageAsync(msg.Chat, RejectMessageText, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: new ReplyKeyboardRemove());

        private Task<Message> SendSuccessMessage(Message msg) =>
            _telegramBotClient.SendTextMessageAsync(msg.Chat, AcceptMessageText, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: new ReplyKeyboardRemove());

        private FileType GetFileType(Message message)
        {
            if (message.Photo != null && message.Photo.Length > 0)
            {
                return FileType.Photo;
            }

            if (message.Sticker != null)
            {
                return FileType.Sticker;
            }

            if (message.Document != null)
            {
                var mimeType = message.Document.MimeType ?? string.Empty;
                var fileName = message.Document.FileName?.ToLower() ?? string.Empty;

                if (mimeType == "video/mp4" || fileName.EndsWith(".mp4") || fileName.EndsWith(".gif"))
                {
                    return FileType.Animation;
                }

                if (mimeType.StartsWith("image/"))
                {
                    return FileType.Photo;
                }
            }

            return FileType.Unknown;
        }
    }
}

