using FluentAssertions;
using Moq;
using UserService.Application.Commands;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;

namespace UserService.Tests
{
    public class LoginCommandHandlerTest
    {
        private readonly Mock<IUserRepository> _repoMock = new();
        private readonly Mock<IJwtService> _jwtServiceMock = new();
        private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
        private readonly LoginCommandHandler _handler;

        public LoginCommandHandlerTest()
        {
            _handler = new LoginCommandHandler(_repoMock.Object, _jwtServiceMock.Object, _passwordHasherMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCredentials_ReturnsToken()
        {
            var user = new User { Id = Guid.NewGuid(), Name = "test", PasswordHash = "hash" };
            _repoMock.Setup(r => r.GetByNameAsync("test", It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            _passwordHasherMock.Setup(p => p.Verify("qwerty", "hash")).Returns(true);
            _jwtServiceMock.Setup(j => j.Generate(user)).Returns("jwt-token");

            var result = await _handler.Handle(new LoginCommand("test", "qwerty"), CancellationToken.None);

            result.Token.Should().Be("jwt-token");
        }

        [Fact]
        public async Task Handle_UserNotFound_ThrowsUnauthorizedException()
        {
            _repoMock.Setup(r => r.GetByNameAsync("test", It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            var act = () => _handler.Handle(new LoginCommand("test", "qwerty"), CancellationToken.None);

            await act.Should().ThrowAsync<UnauthorizedAccessException>();
        }

        [Fact]
        public async Task Handle_WrongPassword_ThrowsUnauthorizedException()
        {
            var user = new User { Id = Guid.NewGuid(), Name = "test", PasswordHash = "hash" };
            _repoMock.Setup(r => r.GetByNameAsync("test", It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            _passwordHasherMock.Setup(p => p.Verify("wrong", "hash")).Returns(false);

            var act = () => _handler.Handle(new LoginCommand("test", "wrong"), CancellationToken.None);

            await act.Should().ThrowAsync<UnauthorizedAccessException>();
        }
    }
}
