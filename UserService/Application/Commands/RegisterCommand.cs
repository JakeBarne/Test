using MediatR;

namespace UserService.Application.Commands
   
{
    public sealed record RegisterCommand(string Name, string Password) : IRequest;

}
