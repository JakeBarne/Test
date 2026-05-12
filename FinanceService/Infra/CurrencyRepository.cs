using FinanceService.Domain.Abstractions;
using FinanceService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Infra
{
    public sealed class CurrencyRepository : ICurrencyRepository
    {
        private readonly AppDbContext _context;

        public CurrencyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Currency>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken ct)
            => await _context.Currencies
            .AsNoTracking()
            .Where(c => ids.Contains(c.Id))
            .ToListAsync(ct);
    }
}
