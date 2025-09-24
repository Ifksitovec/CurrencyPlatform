using FinanceService.Domain.Entities;
using FinanceService.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Tests;

public class FinanceServiceTests
{
    private FinanceDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<FinanceDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new FinanceDbContext(options);

        context.Currencies.AddRange(
            new Currency { Id = 1, Name = "USD" },
            new Currency { Id = 2, Name = "EUR" }
        );

        context.SaveChanges();
        return context;
    }

    [Fact]
    public async Task AddFavoriteAsync_ShouldAddFavorite_WhenNotExists()
    {
        var db = GetDbContext();
        var service = new FinanceService.Infrastructure.Services.FinanceService(db);
        var userId = Guid.NewGuid();

        await service.AddFavoriteAsync(userId, 1);

        var favorite = await db.UserFavoriteCurrencies
            .FirstOrDefaultAsync(f => f.UserId == userId && f.CurrencyId == 1);
        favorite.Should().NotBeNull();
    }

    [Fact]
    public async Task RemoveFavoriteAsync_ShouldRemoveFavorite_WhenExists()
    {
        var db = GetDbContext();
        var userId = Guid.NewGuid();
        db.UserFavoriteCurrencies.Add(new UserFavoriteCurrency { UserId = userId, CurrencyId = 1 });
        await db.SaveChangesAsync();

        var service = new FinanceService.Infrastructure.Services.FinanceService(db);
        await service.RemoveFavoriteAsync(userId, 1);

        var exists = await db.UserFavoriteCurrencies.AnyAsync(f => f.UserId == userId && f.CurrencyId == 1);
        exists.Should().BeFalse();
    }

    [Fact]
    public async Task GetUserFavoriteCurrenciesAsync_ShouldReturnFavorites()
    {
        var db = GetDbContext();
        var userId = Guid.NewGuid();
        db.UserFavoriteCurrencies.Add(new UserFavoriteCurrency { UserId = userId, CurrencyId = 1 });
        await db.SaveChangesAsync();

        var service = new FinanceService.Infrastructure.Services.FinanceService(db);
        var favorites = await service.GetUserFavoriteCurrenciesAsync(userId);

        favorites.Should().HaveCount(1);
        favorites.First().Id.Should().Be(1);
    }
}
