using CurrencyRateMonitor.Database;
using Quartz;

namespace CurrencyRateMonitor
{
    internal class CurrencySaver : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var currencyService = new CurrencyService();
            var currencies = await currencyService.GetCurrentRatesAsync();
            DbHandler.SaveToDb(currencies);
        }
    }
}
