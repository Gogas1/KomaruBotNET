using KomaruBotASPNET.Services;
using Telegram.Bot.Types;

namespace KomaruBotASPNET.Actions.Shared
{
    public class ResetStateAction : CancellationAction<Message>
    {
        private readonly UserService _userService;

        public ResetStateAction(UserService userService)
        {
            _userService = userService;
        }

        public override async Task Execute(Message update, CancellationTokenSource cancellationTokenSource)
        {
            if (update.From == null)
            {
                return;
            }

            if(string.IsNullOrEmpty(update.Text))
            {
                return;
            }

            var parts = update.Text.Trim().Split(' ');

            if(parts.Length < 1)
            {
                return;
            }

            if (parts[0] == "/reset")
            {
                await _userService.SetUserStateAsync(Enums.UserState.Home, update.From.Id);
                cancellationTokenSource.Cancel();
            }
        }
    }
}
