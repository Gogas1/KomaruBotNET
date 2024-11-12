namespace KomaruBotASPNET.Configuration
{
    public class BotConfiguration
    {
        public string Token { get; set; } = string.Empty;
        public Uri WebhookUrl { get; set; }
        public string SecretKey { get; set; } = string.Empty;
        public long AdminId { get; set; }
    }
}
