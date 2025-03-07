using CurrencyRateMonitor.Handlers;
using CurrencyRateMonitor.Service;
using Quartz;

namespace CurrencyRateMonitor.Scheduler
{
    /// <summary>
    /// Класс для выполнения по таймеру
    /// </summary>
    internal class CurrencySaver : IJob
    {
        /// <summary>
        /// Получить значения за сегодня и сохранить в БД
        /// </summary>
        /// <param name="context">Контекст выполнения</param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            var lastDate = DbHandler.GetLastCurrencyRateDate();
            if (lastDate != DateOnly.FromDateTime(DateTime.Now))
            {
                var currencies = await CurrencyService.GetCurrentRatesAsync();
                DbHandler.SaveCurrencyRatesToDb(currencies);
            }
        }
    }
}
