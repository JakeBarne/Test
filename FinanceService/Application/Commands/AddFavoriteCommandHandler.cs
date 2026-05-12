using FinanceService.Domain.Abstractions;
using MediatR;

namespace FinanceService.Application.Commands
{
    public sealed class AddFavoriteCommandHandler : IRequestHandler<AddFavoriteCommand>
    {
        private readonly IFavoriteCurrencyRepository _repository;

        public AddFavoriteCommandHandler(IFavoriteCurrencyRepository repository)
        {
            _repository = repository;
        }

        public Task Handle(AddFavoriteCommand request, CancellationToken ct)
            => _repository.AddAsync(request.UserId, request.CurrencyId, ct);
    }
}
