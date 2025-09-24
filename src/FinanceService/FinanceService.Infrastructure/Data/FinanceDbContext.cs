using FinanceService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Infrastructure.Data
{
    public class FinanceDbContext : DbContext
    {
        public FinanceDbContext(DbContextOptions<FinanceDbContext> options) : base(options) { }
        public DbSet<Currency> Currencies => Set<Currency>();
        public DbSet<UserFavoriteCurrency> UserFavoriteCurrencies => Set<UserFavoriteCurrency>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserFavoriteCurrency>()
                .HasKey(uf => new { uf.UserId, uf.CurrencyId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
