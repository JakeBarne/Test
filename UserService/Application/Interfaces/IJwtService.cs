using UserService.Domain.Entities;

namespace UserService.Application.Interfaces
{
    public interface IJwtService
    {
        string Generate(User user);
    }
}
