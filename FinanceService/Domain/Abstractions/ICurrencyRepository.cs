using FinanceService.Domain.Entities;

namespace FinanceService.Domain.Abstractions
{
    public interface ICurrencyRepository
    {
        Task<IReadOnlyList<Currency>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken ct);
    }
}
