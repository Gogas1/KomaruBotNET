using Telegram.Bot.Types;

namespace KomaruBotASPNET.Extensions
{
    public static class MessageExtensions
    {
        public static bool ValidateMessage(this Message message, bool validateText = false, bool validateCommandPresence = false)
        {
            if(message == null)
            {
                return false;
            }

            if (message.From == null)
            {
                return false;
            }

            if (validateText)
            {
                if (string.IsNullOrEmpty(message.Text))
                {
                    return false;
                }

                if (validateCommandPresence)
                {
                    var parts = message.Text.Trim().Split(' ');

                    if (parts.Length < 1)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
