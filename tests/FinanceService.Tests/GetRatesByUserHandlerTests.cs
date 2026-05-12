using FinanceService.Application.Queries;
using FinanceService.Domain.Abstractions;
using FinanceService.Domain.Entities;
using FluentAssertions;
using Moq;

namespace FinanceService.Tests;

public class GetRatesByUserHandlerTests
{
    private readonly Mock<ICurrencyRepository> _currencyMock = new();
    private readonly Mock<IFavoriteCurrencyRepository> _favoriteCurrencyMock = new();
    private readonly GetRatesByUserHandler _handler;

    public GetRatesByUserHandlerTests()
    {
        _handler = new GetRatesByUserHandler(_currencyMock.Object, _favoriteCurrencyMock.Object);
    }
    [Fact]
    public async Task Handle_UserHasFavorites_ReturnsCorrectRates()
    {
        var userId = Guid.NewGuid();
        var currencyId = Guid.NewGuid();

        _favoriteCurrencyMock.Setup(f => f.GetFavoriteIdsAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Guid> { currencyId });
        _currencyMock.Setup(c => c.GetByIdsAsync(It.IsAny<IEnumerable<Guid>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Currency> { new() { Id = currencyId, Name = "USD", Rate = 91.5m } });
        var result = await _handler.Handle(new GetRatesQuery(userId), CancellationToken.None);

        result.Should().HaveCount(1);
        result[0].Name.Should().Be("USD");
        result[0].Rate.Should().Be(91.5m);
    }

    [Fact]
    public async Task Handle_UserHasNoFavorites_ReturnsEmptyList()
    {
        var userId = Guid.NewGuid();

        _favoriteCurrencyMock.Setup(f => f.GetFavoriteIdsAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Guid>());

        var result = await _handler.Handle(new GetRatesQuery(userId),CancellationToken.None);
        result.Should().BeEmpty();   
    }

}
