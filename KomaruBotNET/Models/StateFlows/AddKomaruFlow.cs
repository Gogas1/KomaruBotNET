using KomaruBotASPNET.Enums;
using Microsoft.EntityFrameworkCore;

namespace KomaruBotASPNET.Models.StateFlows
{
    [Owned]
    public class AddKomaruFlow
    {
        public string FileId { get; set; } = "";
        public string Name { get; set; } = "";
        public FileType FileType { get; set; }
        public List<string> Keywords { get; set; } = new();
    }
}
