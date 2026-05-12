using FluentAssertions;
using Moq;
using UserService.Application.Commands;
using UserService.Application.Interfaces;

namespace UserService.Tests
{
    public class LogoutCommandHandlerTests
    {
        private readonly Mock<ITokenBlacklistService> _blacklistMock = new();
        private readonly LogoutCommandHandler _handler;

        public LogoutCommandHandlerTests()
        {
            _handler = new LogoutCommandHandler(_blacklistMock.Object);
        }

        [Fact]
        public async Task Handle_ValidToken_AddsToBlacklist()
        {
            _blacklistMock.Setup(b => b.AddAsync("my-token", It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            await _handler.Handle(new LogoutCommand("my-token"), CancellationToken.None);

            _blacklistMock.Verify(b => b.AddAsync("my-token", It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
