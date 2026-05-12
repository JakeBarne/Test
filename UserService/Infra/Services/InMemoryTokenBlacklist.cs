using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using UserService.Application.Interfaces;

namespace UserService.Infra.Services
{
    public sealed class InMemoryTokenBlacklist : ITokenBlacklistService
    {
        private readonly ConcurrentDictionary<string, DateTimeOffset> _blacklist = new();
        private readonly TimeSpan _tokenLifetime;

        public InMemoryTokenBlacklist(IOptions<JwtOptions> options)
        {
            _tokenLifetime = TimeSpan.FromMinutes(options.Value.ExpiryMinutes);
        }

        public Task AddAsync(string token, CancellationToken ct)
        {
            _blacklist.TryAdd(token, DateTimeOffset.UtcNow.Add(_tokenLifetime));
            PruneExpired();
            return Task.CompletedTask;
        }

        public Task<bool> IsBlacklistedAsync(string token, CancellationToken ct)
        {
            if (_blacklist.TryGetValue(token, out var expiry))
            {
                if (expiry >= DateTimeOffset.UtcNow)
                    return Task.FromResult(true);
                _blacklist.TryRemove(token, out _);
            }
            return Task.FromResult(false);
        }

        private void PruneExpired()
        {
            var now = DateTimeOffset.UtcNow;
            foreach (var key in _blacklist.Keys)
                if (_blacklist.TryGetValue(key, out var exp) && exp < now)
                    _blacklist.TryRemove(key, out _);
        }
    }
}
