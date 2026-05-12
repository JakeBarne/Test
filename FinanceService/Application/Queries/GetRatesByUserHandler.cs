using FinanceService.Domain.Abstractions;
using MediatR;

namespace FinanceService.Application.Queries
{
    public sealed class GetRatesByUserHandler : IRequestHandler<GetRatesQuery, IReadOnlyList<CurrencyRateDto>>
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IFavoriteCurrencyRepository _favoriteRepository;

        public GetRatesByUserHandler(ICurrencyRepository currencyRepository, IFavoriteCurrencyRepository favoriteRepository)
        {
            _currencyRepository = currencyRepository;
            _favoriteRepository = favoriteRepository;
        }

        public async Task<IReadOnlyList<CurrencyRateDto>> Handle(GetRatesQuery request, CancellationToken ct)
        {
            var favoriteIds = await _favoriteRepository.GetFavoriteIdsAsync(request.UserId, ct);
            if (favoriteIds.Count == 0)
                return [];

            var currencies = await _currencyRepository.GetByIdsAsync(favoriteIds, ct);
            return currencies.Select(c => new CurrencyRateDto(c.Name, c.Rate)).ToList();
        }
    }
}
