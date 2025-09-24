namespace FinanceService.Domain.Entities
{
    public class UserFavoriteCurrency
    {
        public Guid UserId { get; set; }
        public int CurrencyId { get; set; }
    }
}
