using UserService.Domain.Entities;

namespace UserService.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByNameAsync(string name, CancellationToken ct);
        Task<bool> ExistAsync(string name, CancellationToken ct);
        Task AddAsync(User user, CancellationToken ct);
    }
}
