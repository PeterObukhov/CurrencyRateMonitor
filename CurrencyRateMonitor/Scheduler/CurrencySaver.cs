using CurrencyRateMonitor.Handlers;
using CurrencyRateMonitor.Service;
using Quartz;

namespace CurrencyRateMonitor.Scheduler
{
    internal class CurrencySaver : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var currencies = await CurrencyService.GetCurrentRatesAsync();
            DbHandler.SaveToDb(currencies);
        }
    }
}
