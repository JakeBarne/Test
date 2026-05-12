using Microsoft.EntityFrameworkCore;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;

namespace UserService.Infra
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<User?> GetByNameAsync(string name, CancellationToken ct)
            => _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Name == name, ct);

        public Task<bool> ExistAsync(string name, CancellationToken ct)
            => _context.Users.AnyAsync(u => u.Name == name, ct);

        public async Task AddAsync(User user, CancellationToken ct)
        {
            await _context.Users.AddAsync(user, ct);
            await _context.SaveChangesAsync(ct);
        }
    }
}
