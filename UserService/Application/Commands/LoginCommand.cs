using MediatR;

namespace UserService.Application.Commands
{
    public sealed record LoginResult(string Token);
    public sealed record LoginCommand(string Name, string Password) : IRequest<LoginResult>;
}
