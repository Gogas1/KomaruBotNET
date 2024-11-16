using KomaruBotASPNET.DbContexts;
using KomaruBotASPNET.Models;
using Microsoft.EntityFrameworkCore;
using System.Buffers;

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
            var targetGifs = await _context.KomaruGifs.Where(g => EF.Functions.Like(g.Name, $"%{keyword}%") || g.Keywords.Any(kw => EF.Functions.Like(kw.Word, $"%{keyword}%"))).ToListAsync();

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

        public async Task<KomaruGif?> GetGifByKeywords(params string[] keywords)
        {
            var result = await _context.KomaruGifs
                .Include(g => g.Keywords)
                .Select(g => new
                {
                    Gif = g,
                    Score = keywords
                        .Count(keyword => g.Keywords.Select(kw => kw.Word).Contains(keyword, StringComparer.OrdinalIgnoreCase))                   
                })
                .OrderByDescending(result => result.Score)
                .FirstOrDefaultAsync();

            return result?.Gif;
        }

        public async Task<List<KomaruGif>> FullTextSearchAsync(string text)
        {
            text = text.Trim();
            var searchKeywords = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (!searchKeywords.Any())
                return new List<KomaruGif>();

            var containsQuery = string.Join(" OR ", searchKeywords.Select(k => $"\"{k}*\""));

            var query = _context.KomaruGifs
                .TagWith("FullTextSearch")
                .Include(g => g.Keywords)
                .Where(g => EF.Functions.Contains(g.Name, containsQuery) ||
                           g.Keywords.Any(k => EF.Functions.Contains(k.Word, containsQuery)))
                .AsNoTracking();

            var results = await query.ToListAsync();

            return results
                .Select(entity => new
                {
                    Entity = entity,
                    Score = CalculateCombinedScore(searchKeywords, entity)
                })
                .OrderByDescending(x => x.Score)
                .Select(x => x.Entity)
                .ToList();
        }

        private static int CalculateCombinedScore(string[] searchKeywords, KomaruGif gif)
        {
            // Calculate score for both name and keywords list
            int nameScore = CalculateBestMatchScore(searchKeywords, gif.Name);
            int keywordsScore = gif.Keywords.Sum(keyword => CalculateBestMatchScore(searchKeywords, keyword.Word));

            return nameScore + keywordsScore;
        }

        private static int CalculateBestMatchScore(string[] searchKeywords, string field)
        {
            // Calculate cumulative Levenshtein distance for each keyword against the field
            return searchKeywords.Sum(keyword => CalculateLevenshteinDistance(keyword, field));
        }

        private static int CalculateLevenshteinDistance(string a, string b)
        {
            int[,] dp = new int[a.Length + 1, b.Length + 1];

            for (int i = 0; i <= a.Length; i++) dp[i, 0] = i;
            for (int j = 0; j <= b.Length; j++) dp[0, j] = j;

            for (int i = 1; i <= a.Length; i++)
            {
                for (int j = 1; j <= b.Length; j++)
                {
                    int cost = a[i - 1] == b[j - 1] ? 0 : 1;
                    dp[i, j] = Math.Min(
                        Math.Min(dp[i - 1, j] + 1, dp[i, j - 1] + 1),
                        dp[i - 1, j - 1] + cost
                    );
                }
            }

            return dp[a.Length, b.Length];
        }

    }
}
