using FinanceService.Domain.Abstractions;
using MediatR;

namespace FinanceService.Application.Queries
{
    public sealed record GetRatesQuery(Guid UserId) : IRequest<IReadOnlyList<CurrencyRateDto>>;

}
