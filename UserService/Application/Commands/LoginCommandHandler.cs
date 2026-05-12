using MediatR;
using UserService.Application.Interfaces;

namespace UserService.Application.Commands
{
    public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResult>
    {
        private readonly IUserRepository _repository;
        private readonly IJwtService _jwtService;
        private readonly IPasswordHasher _passwordHasher;

        public LoginCommandHandler(IUserRepository repository, IJwtService jwtService, IPasswordHasher passwordHasher)
        {
            _repository = repository;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
        }

        public async Task<LoginResult> Handle(LoginCommand request, CancellationToken ct)
        {
            var user = await _repository.GetByNameAsync(request.Name, ct);
            if (user is null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Неверное имя пользователя или пароль");

            return new LoginResult(_jwtService.Generate(user));
        }
    }
}
