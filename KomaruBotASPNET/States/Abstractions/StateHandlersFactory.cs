using KomaruBotASPNET.Enums;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace KomaruBotASPNET.States.Abstractions
{
    public class StateHandlersFactory<TMessageType>
    {
        private readonly Dictionary<UserState, Func<StateHandlerBase<TMessageType>>> _handlers;

        public StateHandlersFactory(Dictionary<UserState, Func<StateHandlerBase<TMessageType>>> handlers)
        {
            _handlers = handlers;
        }

        public StateHandlerBase<TMessageType>? GetStateHandler(UserState userState)
        {
            if (_handlers.TryGetValue(userState, out var handlerFactory))
            {
                return handlerFactory();
            }

            return null;
        }
    }
}
