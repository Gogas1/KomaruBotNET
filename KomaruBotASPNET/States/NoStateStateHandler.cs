using KomaruBotASPNET.Actions;
using Telegram.Bot.Types;

namespace KomaruBotASPNET.States
{
    public class NoStateStateHandler : StateHandlerBase<Message>
    {
        private readonly IList<ResultAction> _actions;

        public NoStateStateHandler(IList<ResultAction> actions)
        {
            _actions = actions;
        }

        public override async Task Handle(Message message)
        {
            foreach (var action in _actions)
            {
                await action.Execute(message);
            }
        }
    }
}
