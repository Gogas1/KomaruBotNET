namespace KomaruBotASPNET.Models
{
    public class Keyword
    {
        public int Id { get; set; }
        public string Word { get; set; } = string.Empty;
        public KomaruGif Gif { get; set; }
    }
}
