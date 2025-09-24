using FinanceService.Application.Interfaces;
using FinanceService.Domain.Entities;
using FinanceService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Infrastructure.Services
{
    public class FinanceService : IFinanceService
    {
        private readonly FinanceDbContext _dbContext;

        public FinanceService(FinanceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddFavoriteAsync(Guid userId, int currencyId)
        {
            var exists = await _dbContext.UserFavoriteCurrencies
                .AnyAsync(f => f.UserId == userId && f.CurrencyId == currencyId);
            if (!exists)
            {
                _dbContext.UserFavoriteCurrencies.Add(new UserFavoriteCurrency
                {
                    UserId = userId,
                    CurrencyId = currencyId
                });
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task RemoveFavoriteAsync(Guid userId, int currencyId)
        {
            var favorite = await _dbContext.UserFavoriteCurrencies
                .FirstOrDefaultAsync(f => f.UserId == userId && f.CurrencyId == currencyId);
            if (favorite != null)
            {
                _dbContext.UserFavoriteCurrencies.Remove(favorite);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Currency>> GetUserFavoriteCurrenciesAsync(Guid userId)
        {
            var favoriteCurrencyIds = await _dbContext.UserFavoriteCurrencies
                .Where(uf => uf.UserId == userId)
                .Select(uf => uf.CurrencyId)
                .ToListAsync();

            var currencies = await _dbContext.Currencies
                .Where(c => favoriteCurrencyIds.Contains(c.Id))
                .ToListAsync();

            return currencies;
        }
    }
}
