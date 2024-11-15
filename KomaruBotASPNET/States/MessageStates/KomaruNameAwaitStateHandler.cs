using KomaruBotASPNET.Actions;
using KomaruBotASPNET.States.Abstractions;
using Telegram.Bot.Types;

namespace KomaruBotASPNET.States.MessageStates
{
    public class KomaruNameAwaitStateHandler : StateHandlerBase<Message>
    {
        private readonly List<ResultAction<Message>> actions;
        private readonly List<CancellationAction<Message>> beforeActions;

        public KomaruNameAwaitStateHandler(List<CancellationAction<Message>> beforeActions, List<ResultAction<Message>> actions)
        {
            this.beforeActions = beforeActions;
            this.actions = actions;
        }

        public override async Task Handle(Message updateType)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            foreach (var beforeAction in beforeActions)
            {
                if (cancellationTokenSource.IsCancellationRequested)
                {
                    return;
                }

                await beforeAction.Execute(updateType, cancellationTokenSource);
            }

            foreach (var action in actions)
            {
                if (cancellationTokenSource.IsCancellationRequested)
                {
                    return;
                }

                await action.Execute(updateType);
            }
        }
    }
}
