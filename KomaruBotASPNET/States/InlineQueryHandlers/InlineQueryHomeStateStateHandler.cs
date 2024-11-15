using KomaruBotASPNET.Actions;
using KomaruBotASPNET.States.Abstractions;
using Telegram.Bot.Types;

namespace KomaruBotASPNET.States.InlineQueryHandlers
{
    public class InlineQueryHomeStateStateHandler : StateHandlerBase<InlineQuery>
    {
        private readonly List<ResultAction<InlineQuery>> _actions;

        public InlineQueryHomeStateStateHandler(List<ResultAction<InlineQuery>> actions)
        {
            _actions = actions;
        }

        public override async Task Handle(InlineQuery updateType)
        {
            foreach (var action in _actions)
            {
                await action.Execute(updateType);
            }
        }
    }
}
