using KomaruBotASPNET.Enums;

namespace KomaruBotASPNET.Models
{
    public class KomaruGif
    {
        public int Id { get; set; }
        public string TelegramId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public FileType FileType { get; set; }

        public List<Keyword> Keywords { get; set; } = new List<Keyword>();
    }
}
