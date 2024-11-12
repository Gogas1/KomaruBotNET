using KomaruBotASPNET.Services;
using Telegram.Bot.Types;

namespace KomaruBotASPNET.Actions.Home
{
    public class GetKomaruAction : ResultAction
    {
        private readonly GifService _gifService;

        public GetKomaruAction(GifService gifService)
        {
            _gifService = gifService;
        }

        public override Task<Message?> Execute(Message msg)
        {
            throw new NotImplementedException();
        }
    }
}
