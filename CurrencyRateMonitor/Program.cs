using System.Text;

namespace CurrencyRateMonitor
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var service = new CurrencyService();
            var check = await service.GetLastMonthRates();
            Console.ReadKey();
        }
    }
}
