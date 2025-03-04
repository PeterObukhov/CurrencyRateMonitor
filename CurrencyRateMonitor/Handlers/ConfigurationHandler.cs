using Microsoft.Extensions.Configuration;

namespace CurrencyRateMonitor.Handlers
{
    public static class ConfigurationHandler
    {
        public static IConfiguration Сonfiguration { get; private set; }
        public static void PrepareConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json", optional: false);

            Сonfiguration = builder.Build();
        }
    }
}
