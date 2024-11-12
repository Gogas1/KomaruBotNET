using KomaruBotASPNET.Models;
using Microsoft.EntityFrameworkCore;

namespace KomaruBotASPNET.DbContexts
{
    public class KomaruDbContext : DbContext
    {
        public KomaruDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<MyUser> Users { get; set; }
        public DbSet<UserInputState> UserInputStates { get; set; }
        public DbSet<KomaruGif> KomaruGifs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MyUser>()
                .HasOne(u => u.InputState)
                .WithOne(st => st.User)
                .HasForeignKey<UserInputState>(st => st.UserId);

            modelBuilder.Entity<UserInputState>()
                .OwnsOne(us => us.AddKomaruFlow, b =>
                {
                    b.Property(x => x.Keywords);
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}
