using KomaruBotASPNET.Services;
using Telegram.Bot.Types;

namespace KomaruBotASPNET.Actions.MessageActions.NoState
{
    public class SetHomeUserStateAction : ResultAction<Message>
    {
        private readonly UserService _userService;

        public SetHomeUserStateAction(UserService userService)
        {
            _userService = userService;
        }

        public override async Task<Message?> Execute(Message msg)
        {
            if (msg.From == null)
            {
                return null;
            }

            await _userService.SetUserStateAsync(Enums.UserState.Home, msg.From.Id);

            return null;
        }
    }
}
