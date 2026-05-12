using FluentAssertions;
using Moq;
using UserService.Application.Commands;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;

namespace UserService.Tests
{
    public class RegisterCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _mockRepo = new();
        private readonly Mock<IPasswordHasher> _mockPasswordHasher = new();
        private readonly RegisterCommandHandler _handler;

        public RegisterCommandHandlerTests()
        {
            _handler = new RegisterCommandHandler(_mockRepo.Object, _mockPasswordHasher.Object);
        }

        [Fact]
        public async Task Handle_UserDoesNotExist_AddsUser()
        {
            _mockRepo.Setup(r => r.ExistAsync("test", It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _mockPasswordHasher.Setup(p => p.Hash("qwerty")).Returns("hashed");

            await _handler.Handle(new RegisterCommand("test", "qwerty"), CancellationToken.None);

            _mockRepo.Verify(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_UserAlreadyExists_ThrowInvalidOperationException()
        {
            _mockRepo.Setup(r => r.ExistAsync("test", It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var act = () => _handler.Handle(new RegisterCommand("test", "qwerty"), CancellationToken.None);

            await act.Should().ThrowAsync<InvalidOperationException>();
        }
    }
}
