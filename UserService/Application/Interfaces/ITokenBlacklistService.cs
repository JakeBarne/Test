namespace UserService.Application.Interfaces
{
    public interface ITokenBlacklistService
    {
        Task AddAsync(string token, CancellationToken ct);
        Task<bool> IsBlacklistedAsync(string token, CancellationToken ct);
    }
}
