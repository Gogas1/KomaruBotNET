using KomaruBotASPNET.Actions;
using Telegram.Bot.Types;

namespace KomaruBotASPNET.States
{
    public class KomaruFileAwaitStateHandler : StateHandlerBase<Message>
    {
        private List<ResultAction> actions;
        private List<CancellationAction<Message>> beforeActions;

        public KomaruFileAwaitStateHandler(List<ResultAction> actions, List<CancellationAction<Message>> beforeActions)
        {
            this.actions = actions;
            this.beforeActions = beforeActions;
        }

        public override async Task Handle(Message updateType)
        {
            var cTokenSource = new CancellationTokenSource();

            foreach (var beforeAction in beforeActions)
            {
                if(cTokenSource.Token.IsCancellationRequested)
                {
                    break;
                }

                await beforeAction.Execute(updateType, cTokenSource);
            }
           
            foreach (var action in actions)
            {
                if (cTokenSource.Token.IsCancellationRequested)
                {
                    break;
                }

                await action.Execute(updateType);
            }
        }
    }
}
