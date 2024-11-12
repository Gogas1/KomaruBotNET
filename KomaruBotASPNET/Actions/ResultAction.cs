using Telegram.Bot.Types;

namespace KomaruBotASPNET.Actions
{
    public abstract class ResultAction
    {
        public abstract Task<Message?> Execute(Message msg);
    }
}
