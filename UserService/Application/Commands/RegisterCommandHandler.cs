using MediatR;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;

namespace UserService.Application.Commands
{
    public sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand>
    {
        private readonly IUserRepository _repository;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterCommandHandler(IUserRepository repository, IPasswordHasher passwordHasher)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
        }

        public async Task Handle(RegisterCommand request, CancellationToken ct)
        {
            if (await _repository.ExistAsync(request.Name, ct))
                throw new InvalidOperationException($"Пользователь {request.Name} уже существует");

            var user = new User
            {
                Name = request.Name,
                PasswordHash = _passwordHasher.Hash(request.Password)
            };
            await _repository.AddAsync(user, ct);
        }
    }
}
