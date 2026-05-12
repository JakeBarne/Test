using MediatR;
using UserService.Application.Interfaces;

namespace UserService.Application.Commands
{
    public sealed class LogoutCommandHandler : IRequestHandler<LogoutCommand>
    {
        private readonly ITokenBlacklistService _tokenBlacklistService;

        public LogoutCommandHandler(ITokenBlacklistService tokenBlacklistService)
        {
            _tokenBlacklistService = tokenBlacklistService;
        }

        public Task Handle(LogoutCommand request, CancellationToken ct)
            => _tokenBlacklistService.AddAsync(request.Token, ct);
    }
}
