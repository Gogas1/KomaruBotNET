using KomaruBotASPNET.Configuration;
using KomaruBotASPNET.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace KomaruBotASPNET.Extensions
{
    public static class WebApplicationBuilderDbContextExtensions
    {
        public static void AddSqlServerDbContext(this WebApplicationBuilder builder)
        {

            ConnectionStringsOptions connectionStringsOptions = new ConnectionStringsOptions(); 
            builder.Configuration.GetSection("ConnectionStrings").Bind(connectionStringsOptions);

            builder.Services.AddDbContext<KomaruDbContext>(options =>
            {
                options.UseSqlServer(connectionStringsOptions.SqlServer);
            });
        }
    }
}
