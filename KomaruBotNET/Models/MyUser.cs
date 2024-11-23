using KomaruBotASPNET.Enums;

namespace KomaruBotASPNET.Models
{
    public class MyUser
    {
        public int Id { get; set; }
        public long TelegramId { get; set; }
        public UserState UserState { get; set; }

        public UserInputState InputState { get; set; }
    }
}
