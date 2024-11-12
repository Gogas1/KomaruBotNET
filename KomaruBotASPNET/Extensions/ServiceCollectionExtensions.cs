using KomaruBotASPNET.Actions;
using KomaruBotASPNET.Actions.AddKomaru.FileAwait;
using KomaruBotASPNET.Actions.AddKomaru.KeywordsAwait;
using KomaruBotASPNET.Actions.AddKomaru.NameAwait;
using KomaruBotASPNET.Actions.Home;
using KomaruBotASPNET.Actions.NoState;
using KomaruBotASPNET.Actions.Shared;
using KomaruBotASPNET.Configuration;
using KomaruBotASPNET.DbContexts;
using KomaruBotASPNET.Enums;
using KomaruBotASPNET.Services;
using KomaruBotASPNET.States;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Telegram.Bot.Types;

namespace KomaruBotASPNET.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<UpdateService>();
            services.AddScoped<UserService>();
            services.AddTransient<GifService>();
            services.AddTransient<AuthorizationService>();
            services.AddScoped<ReceiverService>();
        }

        public static void AddActions(this IServiceCollection services)
        {
            services.AddTransient<SendHelloGifAction>();
            services.AddTransient<SetHomeUserStateAction>();
            services.AddTransient<SendHomeUsageMessageAction>();
            services.AddTransient<CommandSwitchAction>();
            services.AddTransient<GetKomaruAction>();
            services.AddTransient<InitiateKomaruGifAddingAction>();
            services.AddTransient<GetKomaruFileAction>();
            services.AddTransient<ResetStateAction>();
            services.AddTransient<GetKomaruNameAction>();
            services.AddTransient<GetKomaruKeywordsAction>();
            services.AddTransient<KeywordsEndCAction>();
        }

        public static void AddStateHandlers(this IServiceCollection services)
        {
            services.AddStateHandlersFactories();

            services.AddTransient<NoStateStateHandler>(sp =>
            {
                var actions = new List<ResultAction>
                {
                    sp.GetRequiredService<SendHelloGifAction>(),
                    sp.GetRequiredService<SetHomeUserStateAction>(),
                    sp.GetRequiredService<SendHomeUsageMessageAction>()
                };

                return new NoStateStateHandler(actions);
            });

            services.AddTransient<HomeStateStateHandler>(sp =>
            {
                var actions = new List<ResultAction>
                {
                    sp.GetRequiredService<CommandSwitchAction>(),
                };

                return new HomeStateStateHandler(actions);
            });

            services.AddTransient<KomaruFileAwaitStateHandler>(sp =>
            {
                var actions = new List<ResultAction>
                {
                    sp.GetRequiredService<GetKomaruFileAction>(),
                };

                var beforeActions = new List<CancellationAction<Message>>()
                {
                    sp.GetRequiredService<ResetStateAction>(),
                };

                return new KomaruFileAwaitStateHandler(actions, beforeActions);
            });

            services.AddTransient<KomaruNameAwaitStateHandler>(sp =>
            {
                var actions = new List<ResultAction>
                {
                    sp.GetRequiredService<GetKomaruNameAction>(),
                };

                var beforeActions = new List<CancellationAction<Message>>()
                {
                    sp.GetRequiredService<ResetStateAction>(),
                };

                return new KomaruNameAwaitStateHandler(beforeActions, actions);
            });

            services.AddTransient<KomaruKeywordsAwaitStateHandler>(sp =>
            {
                var actions = new List<ResultAction>
                {
                    sp.GetRequiredService<GetKomaruKeywordsAction>(),
                };

                var beforeActions = new List<CancellationAction<Message>>()
                {
                    sp.GetRequiredService<ResetStateAction>(),
                    sp.GetRequiredService<KeywordsEndCAction>()
                };

                return new KomaruKeywordsAwaitStateHandler(actions, beforeActions);
            });
        }

        private static void AddStateHandlersFactories(this IServiceCollection services)
        {
            services.AddTransient<StateHandlersFactory<Message>>(sp =>
            {
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();

                var messageHandlers = new Dictionary<UserState, Func<StateHandlerBase<Message>>>
                {
                    { UserState.None, () => scopeFactory.CreateScope().ServiceProvider.GetRequiredService<NoStateStateHandler>() },
                    { UserState.Home, () => scopeFactory.CreateScope().ServiceProvider.GetRequiredService<HomeStateStateHandler>() },
                    { UserState.KomaruFileAwait, () => scopeFactory.CreateScope().ServiceProvider.GetRequiredService<KomaruFileAwaitStateHandler>() },
                    { UserState.KomaruNameAwait, () => scopeFactory.CreateScope().ServiceProvider.GetRequiredService<KomaruNameAwaitStateHandler>() },
                    { UserState.KomaruKeywordsAwait, () => scopeFactory.CreateScope().ServiceProvider.GetRequiredService<KomaruKeywordsAwaitStateHandler>() },
                };

                return new StateHandlersFactory<Message>(messageHandlers);
            });
        }
        public static void AddSqlServerDbContext(this IServiceCollection services)
        {
            services.AddDbContext<KomaruDbContext>((sp, options) =>
            {
                ConnectionStringsOptions? connectionStringsOptions = sp.GetService<IOptions<ConnectionStringsOptions>>()?.Value;
                ArgumentNullException.ThrowIfNull(connectionStringsOptions);

                options.UseSqlServer(connectionStringsOptions.SqlServer);
            });
        }
    }
}
