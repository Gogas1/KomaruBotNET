using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace KomaruBotASPNET.States.Abstractions
{
    public abstract class StateHandlerBase<TUpdateType>
    {
        public abstract Task Handle(TUpdateType updateType);

        public virtual Task BeforeHandle(TUpdateType updateType)
        {
            return Task.CompletedTask;
        }
    }
}
