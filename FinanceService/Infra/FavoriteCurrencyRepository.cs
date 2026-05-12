using FinanceService.Domain.Abstractions;
using FinanceService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Infra
{
    public sealed class FavoriteCurrencyRepository : IFavoriteCurrencyRepository
    {
        private readonly AppDbContext _context;

        public FavoriteCurrencyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Guid>> GetFavoriteIdsAsync(Guid userId, CancellationToken ct)
            => await _context.FavoriteCurrency
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(x => x.CurrencyId)
                .ToListAsync(ct);

        public async Task AddAsync(Guid userId, Guid currencyId, CancellationToken ct)
        {
            var exists = await _context.FavoriteCurrency
                .AnyAsync(x => x.UserId == userId && x.CurrencyId == currencyId, ct);
            if (exists)
                return;

            _context.FavoriteCurrency.Add(new UserFavoriteCurrency { UserId = userId, CurrencyId = currencyId });
            await _context.SaveChangesAsync(ct);
        }

        public async Task RemoveAsync(Guid userId, Guid currencyId, CancellationToken ct)
        {
            var row = await _context.FavoriteCurrency
                .FirstOrDefaultAsync(x => x.UserId == userId && x.CurrencyId == currencyId, ct);
            if (row is null)
                return;

            _context.FavoriteCurrency.Remove(row);
            await _context.SaveChangesAsync(ct);
        }
    }
}
