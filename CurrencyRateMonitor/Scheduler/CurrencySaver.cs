using CurrencyRateMonitor.Handlers;
using CurrencyRateMonitor.Service;
using Quartz;

namespace CurrencyRateMonitor.Scheduler
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
