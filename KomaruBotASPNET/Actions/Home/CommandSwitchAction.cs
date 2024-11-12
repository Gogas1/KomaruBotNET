using KomaruBotASPNET.Actions.Shared;
using Telegram.Bot.Types;

namespace KomaruBotASPNET.Actions.Home
{
    public class CommandSwitchAction : ResultAction
    {
        private readonly SendHomeUsageMessageAction botUsageAction;
        private readonly GetKomaruAction getKomaruAction;
        private readonly InitiateKomaruGifAddingAction addKomaruAction;

        public CommandSwitchAction(SendHomeUsageMessageAction botUsageAction, GetKomaruAction getKomaruAction, InitiateKomaruGifAddingAction addKomaruAction)
        {
            this.botUsageAction = botUsageAction;
            this.getKomaruAction = getKomaruAction;
            this.addKomaruAction = addKomaruAction;
        }

        public async override Task<Message?> Execute(Message msg)
        {
            if(msg.Text == null)
            {
                return null;
            }

            string command = msg.Text.Trim().Split(' ')[0];

            ResultAction resultAction = msg.Text switch
            {
                "/help" => botUsageAction,
                "/komaru" => getKomaruAction,
                "/addkomaru" => addKomaruAction,
                _ => botUsageAction,
            };

            return await resultAction.Execute(msg);
        }
    }
}
