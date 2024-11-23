using KomaruBotASPNET.Actions;
using KomaruBotASPNET.States.Abstractions;
using Telegram.Bot.Types;

namespace KomaruBotASPNET.States.MessageStates
{
    public class KomaruKeywordsAwaitStateHandler : StateHandlerBase<Message>
    {
        private readonly List<ResultAction<Message>> actions;
        private readonly List<CancellationAction<Message>> beforeActions;

        public KomaruKeywordsAwaitStateHandler(List<ResultAction<Message>> actions, List<CancellationAction<Message>> beforeActions)
        {
            this.actions = actions;
            this.beforeActions = beforeActions;
        }

        public override async Task Handle(Message updateType)
        {
            CancellationTokenSource ctSource = new CancellationTokenSource();

            foreach (var beforeAction in beforeActions)
            {
                if (ctSource.IsCancellationRequested)
                {
                    return;
                }

                await beforeAction.Execute(updateType, ctSource);
            }

            foreach (var action in actions)
            {
                if (ctSource.IsCancellationRequested) return;

                await action.Execute(updateType);
            }
        }
    }
}
