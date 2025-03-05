using CurrencyRateMonitor.Handlers;
using CurrencyRateMonitor.Scheduler;
using CurrencyRateMonitor.Service;
using System.Text;

namespace CurrencyRateMonitor
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            ConfigurationHandler.PrepareConfiguration();

            DbHandler.ApplyMigration();

            CurrencyService.InitializeService();

            await Task.Run(CurrencyScheduler.Start);

            Console.WriteLine("Сервис фоновой выгрузки запущен и будет срабатывать каждый день в " + ConfigurationHandler.Сonfiguration.GetSection("CronTime").Value);

            Console.WriteLine("Загрузить данные за последний месяц? (Y/N)");
            if (Console.ReadLine().ToLower() == "y")
            {
                DbHandler.SaveToDb(await CurrencyService.GetLastMonthRatesAsync());
                Console.WriteLine("Сохранение успешно");
            }

            Console.WriteLine("Нажмите клавишу Enter для завершения работы");
            Console.ReadLine();
        }
    }
}
