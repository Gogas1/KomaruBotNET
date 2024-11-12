using KomaruBotASPNET.Configuration;

namespace KomaruBotASPNET.Extensions
{
    public static class WebApplicationBuilderConfigurationExtensions
    {
        public static void AddBotConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<BotConfiguration>(builder.Configuration.GetSection("Telegram"));
        }
    }
}
