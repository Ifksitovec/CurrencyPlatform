using FinanceService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace FinanceService.Api.Services
{
    public class CurrencyUpdaterService : BackgroundService
    {
        private readonly IServiceProvider _services;

        public CurrencyUpdaterService(IServiceProvider services) => _services = services;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<FinanceDbContext>();

                var xml = await new HttpClient().GetStringAsync("http://www.cbr.ru/scripts/XML_daily.asp");
                var doc = XDocument.Parse(xml);

                var currencies = doc.Descendants("Valute").Select(x => new FinanceService.Domain.Entities.Currency
                {
                    Name = x.Element("CharCode")!.Value,
                    Rate = decimal.Parse(x.Element("Value")!.Value.Replace(',', '.'))
                }).ToList();

                foreach (var cur in currencies)
                {
                    var existing = await db.Currencies.FirstOrDefaultAsync(c => c.Name == cur.Name);
                    if (existing != null) existing.Rate = cur.Rate;
                    else db.Currencies.Add(cur);
                }

                await db.SaveChangesAsync();
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}
