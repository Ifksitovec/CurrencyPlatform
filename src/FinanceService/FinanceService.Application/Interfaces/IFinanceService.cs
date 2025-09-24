using FinanceService.Domain.Entities;

namespace FinanceService.Application.Interfaces
{
    public interface IFinanceService
    {
        Task AddFavoriteAsync(Guid userId, int currencyId);
        Task RemoveFavoriteAsync(Guid userId, int currencyId);
        Task<IEnumerable<Currency>> GetUserFavoriteCurrenciesAsync(Guid userId);
    }
}
