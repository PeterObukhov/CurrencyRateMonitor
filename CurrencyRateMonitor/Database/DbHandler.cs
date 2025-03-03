using CurrencyRateMonitor.Models;

namespace CurrencyRateMonitor.Database
{
    public class DbHandler
    {
        public static void SaveToDb(IEnumerable<CurrencyRate> currencyRates)
        {
            using (CurrencyDbContext db = new CurrencyDbContext())
            {
                db.CurrencyRates.AddRange(currencyRates);
                db.SaveChanges();
            }
        }
    }
}
