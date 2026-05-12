using FinanceService.Domain.Abstractions;
using MediatR;

namespace FinanceService.Application.Commands
{
    public sealed class RemoveFavoriteCommandHandler : IRequestHandler<RemoveFavoriteCommand>
    {
        private readonly IFavoriteCurrencyRepository _repository;

        public RemoveFavoriteCommandHandler(IFavoriteCurrencyRepository repository)
        {
            _repository = repository;
        }

        public Task Handle(RemoveFavoriteCommand request, CancellationToken ct)
            => _repository.RemoveAsync(request.UserId, request.CurrencyId, ct);
    }
}
