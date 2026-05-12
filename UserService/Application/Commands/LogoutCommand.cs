using MediatR;

namespace UserService.Application.Commands
{
    public sealed record LogoutCommand(string Token) : IRequest;
    
}
