using MediatR;

namespace FinanceService.Application.Commands
{
    public sealed record RemoveFavoriteCommand(Guid UserId, Guid CurrencyId) : IRequest;
}
