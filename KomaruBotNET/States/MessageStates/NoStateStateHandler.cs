using KomaruBotASPNET.Actions;
using KomaruBotASPNET.States.Abstractions;
using Telegram.Bot.Types;

namespace KomaruBotASPNET.States.MessageStates
{
    public class NoStateStateHandler : StateHandlerBase<Message>
    {
        private readonly IList<ResultAction<Message>> _actions;

        public NoStateStateHandler(IList<ResultAction<Message>> actions)
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
