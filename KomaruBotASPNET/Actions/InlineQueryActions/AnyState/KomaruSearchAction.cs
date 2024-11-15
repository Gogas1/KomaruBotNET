using KomaruBotASPNET.Enums;
using KomaruBotASPNET.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineQueryResults;

namespace KomaruBotASPNET.Actions.InlineQueryActions.AnyState
{
    public class KomaruSearchAction : ResultAction<InlineQuery>
    {
        private readonly GifService gifService;
        private readonly ITelegramBotClient telegramBotClient;

        public KomaruSearchAction(GifService gifService, ITelegramBotClient telegramBotClient)
        {
            this.gifService = gifService;
            this.telegramBotClient = telegramBotClient;
        }

        public override async Task<Message?> Execute(InlineQuery updateType)
        {
            if(string.IsNullOrEmpty(updateType.Query))
            {
                return null;
            }

            var gifs = await gifService.FullTextSearchAsync(updateType.Query);
            var results = new List<InlineQueryResult>();

            int index = 0;

            foreach (var mediaItem in gifs)
            {
                InlineQueryResult? result = mediaItem.FileType switch
                {
                    Enums.FileType.Animation => new InlineQueryResultCachedMpeg4Gif(
                        id: index.ToString(),
                        mpeg4FileId: mediaItem.TelegramId
                    )
                    { Title = "Animation" },

                    FileType.Photo => new InlineQueryResultCachedPhoto(
                        id: index.ToString(),
                        photoFileId: mediaItem.TelegramId
                    )
                    { Title = "Photo" },

                    FileType.Sticker => new InlineQueryResultCachedSticker(
                        id: index.ToString(),
                        stickerFileId: mediaItem.TelegramId
                    ),

                    _ => null
                };

                if (result != null)
                    results.Add(result);
                index++;
            }

            await telegramBotClient.AnswerInlineQueryAsync(updateType.Id, results);

            return null;
        }
    }
}
