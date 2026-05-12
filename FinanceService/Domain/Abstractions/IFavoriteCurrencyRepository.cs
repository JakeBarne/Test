namespace FinanceService.Domain.Abstractions
{
    public interface IFavoriteCurrencyRepository
    {
        Task<IReadOnlyList<Guid>> GetFavoriteIdsAsync(Guid userId, CancellationToken ct);
        Task AddAsync(Guid userId, Guid currencyId, CancellationToken ct);
        Task RemoveAsync(Guid userId, Guid currencyId, CancellationToken ct);
    }
}
