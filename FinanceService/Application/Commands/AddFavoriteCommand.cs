using MediatR;

namespace FinanceService.Application.Commands
{
    public sealed record AddFavoriteCommand(Guid UserId, Guid CurrencyId) : IRequest;
}
