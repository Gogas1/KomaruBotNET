﻿using KomaruBotASPNET.Actions;
using Telegram.Bot.Types;

namespace KomaruBotASPNET.States
{
    public class HomeStateStateHandler : StateHandlerBase<Message>
    {
        private readonly IList<ResultAction> _actions;

        public HomeStateStateHandler(IList<ResultAction> actions)
        {
            _actions = actions;
        }

        public override async Task Handle(Message updateType)
        {
            foreach (var action in _actions)
            {
                await action.Execute(updateType);
            }
        }
    }
}
