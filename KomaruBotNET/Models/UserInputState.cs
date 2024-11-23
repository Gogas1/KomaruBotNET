using KomaruBotASPNET.Models.StateFlows;

namespace KomaruBotASPNET.Models
{
    public class UserInputState
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public MyUser User { get; set; }

        public AddKomaruFlow AddKomaruFlow { get; set; } = new AddKomaruFlow();
    }
}
