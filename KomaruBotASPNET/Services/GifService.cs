using KomaruBotASPNET.DbContexts;
using KomaruBotASPNET.Models;
using Microsoft.EntityFrameworkCore;

namespace KomaruBotASPNET.Services
{
    public class GifService
    {
        private readonly KomaruDbContext _context;

        public GifService(KomaruDbContext context)
        {
            _context = context;
        }

        public async Task<KomaruGif?> GetGifByKeyword(string keyword)
        {
            var targetGifs = await _context.KomaruGifs.Where(g => g.Name.Contains(keyword) || g.Keywords.Contains(keyword)).ToListAsync();

            if(targetGifs.Any())
            {
                int size = targetGifs.Count;
                return targetGifs[Random.Shared.Next(0, targetGifs.Count)];
            }
            else { return null; }
        }

        public async Task<KomaruGif?> GetRandomGif()
        {
            int gifCount = await _context.KomaruGifs.CountAsync();

            if (gifCount > 0)
            {
                int randomIndex = Random.Shared.Next(1, gifCount);

                var gif = await _context.KomaruGifs.Skip(randomIndex).FirstOrDefaultAsync();
                return gif ?? null;
            }
            else
            {
                return null;
            }
        }

        public async Task<KomaruGif> CreateGif(KomaruGif komaruGif)
        {
            _context.KomaruGifs.Add(komaruGif);
            await _context.SaveChangesAsync();

            return komaruGif;
        }
    }
}
