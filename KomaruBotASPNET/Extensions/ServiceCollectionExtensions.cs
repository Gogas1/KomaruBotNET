using KomaruBotASPNET.Actions;
using KomaruBotASPNET.Actions.InlineQueryActions.AnyState;
using KomaruBotASPNET.Actions.MessageActions.AddKomaru.FileAwait;
using KomaruBotASPNET.Actions.MessageActions.AddKomaru.KeywordsAwait;
using KomaruBotASPNET.Actions.MessageActions.AddKomaru.NameAwait;
using KomaruBotASPNET.Actions.MessageActions.Home;
using KomaruBotASPNET.Actions.MessageActions.NoState;
using KomaruBotASPNET.Actions.MessageActions.Shared;
using KomaruBotASPNET.Configuration;
using KomaruBotASPNET.DbContexts;
using KomaruBotASPNET.Enums;
using KomaruBotASPNET.Services;
using KomaruBotASPNET.States.Abstractions;
using KomaruBotASPNET.States.InlineQueryHandlers;
using KomaruBotASPNET.States.MessageStates;
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

            services.AddTransient<KomaruSearchAction>();
        }

        public static void AddStateHandlers(this IServiceCollection services)
        {
            services.AddStateHandlersFactories();

            services.AddTransient<NoStateStateHandler>(sp =>
            {
                var actions = new List<ResultAction<Message>>
                {
                    sp.GetRequiredService<SendHelloGifAction>(),
                    sp.GetRequiredService<SetHomeUserStateAction>(),
                    sp.GetRequiredService<SendHomeUsageMessageAction>()
                };

                return new NoStateStateHandler(actions);
            });

            services.AddTransient<HomeStateStateHandler>(sp =>
            {
                var actions = new List<ResultAction<Message>>
                {
                    sp.GetRequiredService<CommandSwitchAction>(),
                };

                return new HomeStateStateHandler(actions);
            });

            services.AddTransient<KomaruFileAwaitStateHandler>(sp =>
            {
                var actions = new List<ResultAction<Message>>
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
                var actions = new List<ResultAction<Message>>
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
                var actions = new List<ResultAction<Message>>
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

            services.AddTransient<InlineQueryHomeStateStateHandler>(sp =>
            {
                List<ResultAction<InlineQuery>> actions = new List<ResultAction<InlineQuery>>
                {
                    sp.GetRequiredService<KomaruSearchAction>(),
                };

                return new InlineQueryHomeStateStateHandler(actions);
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

            services.AddTransient<StateHandlersFactory<InlineQuery>>(sp =>
            {
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();

                var inlineQueryHandlers = new Dictionary<UserState, Func<StateHandlerBase<InlineQuery>>>
                {
                    { UserState.Home, () => scopeFactory.CreateScope().ServiceProvider.GetRequiredService<InlineQueryHomeStateStateHandler>() },
                };

                return new StateHandlersFactory<InlineQuery>(inlineQueryHandlers);
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
