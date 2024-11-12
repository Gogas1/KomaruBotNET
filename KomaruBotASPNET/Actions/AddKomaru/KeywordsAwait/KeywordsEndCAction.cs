﻿using KomaruBotASPNET.Extensions;
using KomaruBotASPNET.Models;
using KomaruBotASPNET.Models.StateFlows;
using KomaruBotASPNET.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KomaruBotASPNET.Actions.AddKomaru.KeywordsAwait
{
    public class KeywordsEndCAction : CancellationAction<Message>
    {
        private readonly GifService gifService;
        private readonly UserService userService;
        private readonly ITelegramBotClient telegramBotClient;

        private const string FailureMessage = "При попытке добавить кумару файл произошла ошибка. Сбрось состояние с помощью /reset и попробуй заново";
        private const string SuccessMessage = "Новый кумару файл под названием {0} с ключевыми словами: {1}, был добавлен";

        public KeywordsEndCAction(GifService gifService, UserService userService, ITelegramBotClient telegramBotClient)
        {
            this.gifService = gifService;
            this.userService = userService;
            this.telegramBotClient = telegramBotClient;
        }

        public override async Task Execute(Message update, CancellationTokenSource cancellationToken)
        {
            if(!update.ValidateMessage(true, true))
            {                
                return;
            }

            var parts = update.Text!.Trim().Split(' ');

            if (parts[0] == "/end")
            {
                AddKomaruFlow? addKomaruFlow = await userService.GetUserStateObject(update.From!.Id, us => us.AddKomaruFlow);

                if(addKomaruFlow == null)
                {
                    await telegramBotClient.SendTextMessageAsync(update.Chat.Id, FailureMessage);
                    return;
                }

                KomaruGif komaruGif = new KomaruGif()
                {
                    TelegramId = addKomaruFlow.FileId,
                    Name = addKomaruFlow.Name,
                    Keywords = new(string.Join(' ', addKomaruFlow.Keywords)),
                    FileType = addKomaruFlow.FileType,
                };

                await gifService.CreateGif(komaruGif);
                await userService.SetUserStateAsync(Enums.UserState.Home, update.From.Id);
                await telegramBotClient.SendTextMessageAsync(update.Chat.Id, string.Format(SuccessMessage, komaruGif.Name, komaruGif.Keywords));
                await SendKomaruFile(komaruGif, update.Chat);
                cancellationToken.Cancel();
            }

            return;
        }

        public async Task SendKomaruFile(KomaruGif komaruGif, Chat chat)
        {
            switch (komaruGif.FileType)
            {
                case Enums.FileType.Sticker:
                    await telegramBotClient.SendStickerAsync(chat.Id, komaruGif.TelegramId);
                    break;
                case Enums.FileType.Photo:
                    await telegramBotClient.SendPhotoAsync(chat.Id, komaruGif.TelegramId);
                    break;
                case Enums.FileType.Animation:
                    await telegramBotClient.SendAnimationAsync(chat.Id, komaruGif.TelegramId);
                    break;
                default:
                    await telegramBotClient.SendDocumentAsync(chat.Id, komaruGif.TelegramId);
                    break;
            }
        }
    }
}