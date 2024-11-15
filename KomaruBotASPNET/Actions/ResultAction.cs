using Telegram.Bot.Types;

namespace KomaruBotASPNET.Actions
{
    public abstract class ResultAction<TUpdateType>
    {
        public abstract Task<Message?> Execute(TUpdateType updateType);
    }
}
