using KomaruBotASPNET.Actions;
using Telegram.Bot.Types;

namespace KomaruBotASPNET.States
{
    public class KomaruKeywordsAwaitStateHandler : StateHandlerBase<Message>
    {
        private readonly List<ResultAction> actions;
        private readonly List<CancellationAction<Message>> beforeActions;

        public KomaruKeywordsAwaitStateHandler(List<ResultAction> actions, List<CancellationAction<Message>> beforeActions)
        {
            this.actions = actions;
            this.beforeActions = beforeActions;
        }

        public override async Task Handle(Message updateType)
        {
            CancellationTokenSource ctSource = new CancellationTokenSource();
            
            foreach (var beforeAction in beforeActions)
            {
                if(ctSource.IsCancellationRequested)
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
