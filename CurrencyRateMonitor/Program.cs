using CurrencyRateMonitor.Handlers;
using CurrencyRateMonitor.Scheduler;
using CurrencyRateMonitor.Service;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System.Text;

namespace CurrencyRateMonitor
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            ConfigurationHandler.PrepareConfiguration();

            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            ILogger serviceLogger = loggerFactory.CreateLogger<CurrencyService>();
            ILogger dbLogger = loggerFactory.CreateLogger<DbHandler>();

            DbHandler.InitializeDB(dbLogger);
            CurrencyService.InitializeService(serviceLogger);
            DbHandler.SaveCurrencyCodesToDb(await CurrencyService.GetCurrencyCodes());

            await Task.Run(CurrencyScheduler.Start);
            Console.WriteLine("Сервис фоновой выгрузки запущен и будет срабатывать каждый день в " + ConfigurationHandler.Сonfiguration.GetSection("CronTime").Value);

            Console.WriteLine("Загрузить данные за последний месяц? (Y/N)");
            
            if (Console.ReadLine().ToLower() == "y")
            {
                DbHandler.SaveCurrencyRatesToDb(await CurrencyService.GetLastMonthRatesAsync());
                Console.WriteLine("Сохранение успешно");
            }

            Console.WriteLine("Нажмите клавишу Enter для завершения работы (фоновый сервис остановится)");
            Console.ReadLine();
        }
    }
}
