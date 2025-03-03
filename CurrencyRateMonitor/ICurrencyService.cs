using CurrencyRateMonitor.Models;

namespace CurrencyRateMonitor
{
    internal interface ICurrencyService
    {
        public Task<IEnumerable<CurrencyRate>> GetCurrentRatesAsync();
        public Task<IEnumerable<CurrencyRate>> GetLastMonthRatesAsync();
    }
}
