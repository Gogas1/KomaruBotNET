using KomaruBotASPNET.DbContexts;
using KomaruBotASPNET.Enums;
using KomaruBotASPNET.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace KomaruBotASPNET.Services
{
    public class UserService
    {
        private readonly KomaruDbContext _context;

        public UserService(KomaruDbContext context)
        {
            _context = context;
        }

        public async Task<UserState> GetUserStateByTelegramIdAsync(long telegramId)
        {
            var targerUser = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.TelegramId == telegramId);

            if (targerUser == null)
            {
                targerUser = new MyUser()
                {
                    TelegramId = telegramId,
                    UserState = UserState.None
                };

                _context.Users.Add(targerUser);
                await _context.SaveChangesAsync();
            }

            return targerUser.UserState;
        }

        public async Task SetUserStateAsync(UserState newState, long userTelegramId)
        {
            var targerUser = await _context.Users
                .FirstOrDefaultAsync(u => u.TelegramId == userTelegramId)
                ?? new MyUser { TelegramId = userTelegramId };

            targerUser.UserState = newState;

            _context.Update(targerUser);
            await _context.SaveChangesAsync();
        }

        public async Task SetUserStateInputStateAsync(long userTelegramId, Action<UserInputState> handler)
        {
            var targerUser = await _context.Users
                .Include(u => u.InputState)
                .FirstOrDefaultAsync(u => u.TelegramId == userTelegramId)
                ?? new MyUser
                {
                    TelegramId = userTelegramId,
                    InputState = new UserInputState()
                };

            if (targerUser.InputState == null)
            {
                targerUser.InputState = new UserInputState();
            }

            handler(targerUser.InputState);
            _context.Users.Update(targerUser);
            await _context.SaveChangesAsync();
        }

        public async Task<TUserState?> GetUserStateObject<TUserState>(long userTelegramId, Func<UserInputState, TUserState> func)
        {
            var targetUser = await _context.Users
                .Include(u => u.InputState)
                .FirstOrDefaultAsync(u => u.TelegramId == userTelegramId);

            if(targetUser == null)
            {
                return default;
            }

            return func(targetUser.InputState);
        }
    }
}
