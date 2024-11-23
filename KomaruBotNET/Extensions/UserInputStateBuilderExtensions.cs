using KomaruBotASPNET.Models;
using KomaruBotASPNET.Models.StateFlows;

namespace KomaruBotASPNET.Extensions
{
    public static class UserInputStateBuilderExtensions
    {
        public static UserInputState SetAddKomaruFlow(this UserInputState userInputState, AddKomaruFlow addKomaruFlow)
        {
            if(userInputState == null)
            {
                userInputState = new UserInputState();
            }

            userInputState.AddKomaruFlow = new AddKomaruFlow
            {
                FileId = addKomaruFlow.FileId,
                Keywords = new List<string>(addKomaruFlow.Keywords),
                Name = addKomaruFlow.Name,
                FileType = addKomaruFlow.FileType
            };

            return userInputState;
        }
    }
}
