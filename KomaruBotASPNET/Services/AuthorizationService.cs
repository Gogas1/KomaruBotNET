using KomaruBotASPNET.Configuration;
using Microsoft.Extensions.Options;

namespace KomaruBotASPNET.Services
{
    public class AuthorizationService
    {
        public AuthorizationService(IOptions<BotConfiguration> botConfig)
        {
            _botConfig = botConfig.Value;

            ArgumentNullException.ThrowIfNull(_botConfig);
        }

        private BotConfiguration _botConfig;

        public bool IsAdminAccount(long telegramUserId)
        {
            return _botConfig.AdminIds.Contains(telegramUserId);
        } 
    }
}
