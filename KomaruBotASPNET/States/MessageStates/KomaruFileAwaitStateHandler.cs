using KomaruBotASPNET.Actions;
using KomaruBotASPNET.States.Abstractions;
using Telegram.Bot.Types;

namespace KomaruBotASPNET.States.MessageStates
{
    public class KomaruFileAwaitStateHandler : StateHandlerBase<Message>
    {
        private List<ResultAction<Message>> actions;
        private List<CancellationAction<Message>> beforeActions;

        public KomaruFileAwaitStateHandler(List<ResultAction<Message>> actions, List<CancellationAction<Message>> beforeActions)
        {
            this.actions = actions;
            this.beforeActions = beforeActions;
        }

        public override async Task Handle(Message updateType)
        {
            var cTokenSource = new CancellationTokenSource();

            foreach (var beforeAction in beforeActions)
            {
                if (cTokenSource.Token.IsCancellationRequested)
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
