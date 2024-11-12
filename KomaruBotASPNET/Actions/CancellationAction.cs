namespace KomaruBotASPNET.Actions
{
    public abstract class CancellationAction<TUpdateType>
    {
        public abstract Task Execute(TUpdateType update, CancellationTokenSource cancellationToken);
    }
}
