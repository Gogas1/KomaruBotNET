using KomaruBotASPNET.Abstractions;
using Telegram.Bot;

namespace KomaruBotASPNET.Services
{
    public class ReceiverService(ITelegramBotClient botClient, UpdateService updateService) : ReceiverServiceBase<UpdateService>(botClient, updateService)
    {
    }
}
