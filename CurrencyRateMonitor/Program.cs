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

            await Task.Run(() => CurrencyScheduler.Start());

            var service = new CurrencyService();

            do
            {
                Console.WriteLine("Загрузить данные за последний месяц? (Y/N)");
                if (Console.ReadLine().ToLower() == "y")
                {
                    DbHandler.SaveToDb(await service.GetLastMonthRatesAsync());
                    Console.WriteLine("Сохранение успешно");
                }

                Console.WriteLine("Нажмите ESC для выхода или любую другую клавишу для повтора");
            } while (Console.ReadKey().Key != ConsoleKey.Escape);

            Console.WriteLine("0Выход из загрузки за месяц, ежедневная выгрузка работает");
            Console.ReadKey();
        }
    }
}
