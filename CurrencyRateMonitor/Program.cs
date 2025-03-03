using CurrencyRateMonitor.Database;
using System.Text;

namespace CurrencyRateMonitor
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var service = new CurrencyService();
            DbHandler.SaveToDb(await service.GetLastMonthRatesAsync());
            
            await Task.Run(() => CurrencyScheduler.Start());
            
            Console.WriteLine("Finished");
            Console.ReadKey();
        }
    }
}
