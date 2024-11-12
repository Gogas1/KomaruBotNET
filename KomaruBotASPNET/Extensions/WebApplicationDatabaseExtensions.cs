using KomaruBotASPNET.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace KomaruBotASPNET.Extensions
{
    public static class WebApplicationDatabaseExtensions
    {
        public static async void MigrateDb(this IHost app)
        {
            using var scope = app.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetService<KomaruDbContext>();

            ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));

            await dbContext.Database.MigrateAsync();
        }
    }
}
