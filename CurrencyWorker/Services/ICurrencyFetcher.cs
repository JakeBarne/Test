using CurrencyWorker.Models;

namespace CurrencyWorker.Services
{
    public interface ICurrencyFetcher
    {
        Task<IReadOnlyList<CurrencyDTO>> FetchAsync(CancellationToken ct);
    }
}
