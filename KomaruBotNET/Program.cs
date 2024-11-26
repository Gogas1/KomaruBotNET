
using KomaruBotASPNET.Configuration;
using KomaruBotASPNET.Extensions;
using KomaruBotASPNET.Services;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace KomaruBotASPNET
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Task.WaitAll(SetupLongpolling(args));
        }

        public static async Task SetupLongpolling(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {                
                services.Configure<BotConfiguration>(context.Configuration.GetSection("Telegram"));
                services.Configure<ConnectionStringsOptions>(context.Configuration.GetSection("ConnectionStrings"));

                services.AddHttpClient("telegram_bot_client").RemoveAllLoggers()
                        .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
                        {
                            BotConfiguration? botConfiguration = sp.GetService<IOptions<BotConfiguration>>()?.Value;
                            ArgumentNullException.ThrowIfNull(botConfiguration);
                            TelegramBotClientOptions options = new(botConfiguration.Token);
                            var bot = new TelegramBotClient(options, httpClient);
                            Task.WaitAny(bot.DeleteWebhook());
                            return bot;
                        });

                services.AddSqlServerDbContext();

                services.AddServices();
                services.AddActions();
                services.AddStateHandlers();

                services.AddHostedService<PollingService>();
            })
            .Build();

            host.MigrateDb();

            await host.RunAsync();
        }
    }
}
