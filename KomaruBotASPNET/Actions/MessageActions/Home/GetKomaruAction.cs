using KomaruBotASPNET.Services;
using Telegram.Bot.Types;

namespace KomaruBotASPNET.Actions.MessageActions.Home
{
    public class GetKomaruAction : ResultAction<Message>
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
