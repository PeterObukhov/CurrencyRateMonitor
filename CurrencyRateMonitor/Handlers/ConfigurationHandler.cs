using Microsoft.Extensions.Configuration;

namespace CurrencyRateMonitor.Handlers
{
    /// <summary>
    /// Класс для работы с конфигурацией
    /// </summary>
    public static class ConfigurationHandler
    {
        public static IConfiguration Сonfiguration { get; private set; }
        
        /// <summary>
        /// Создание конфигурации приложения
        /// </summary>
        public static void PrepareConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json", optional: false);

            Сonfiguration = builder.Build();
        }
    }
}
