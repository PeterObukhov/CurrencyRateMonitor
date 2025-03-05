using CurrencyRateMonitor.Models;
using Npgsql;
using CurrencyRateMonitor.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CurrencyRateMonitor.Handlers
{
    /// <summary>
    /// Класс для работы с БД
    /// </summary>
    public class DbHandler
    {
        private static ILogger _logger;

        /// <summary>
        /// Миграция базы данных
        /// </summary>
        /// <param name="logger">Логгер для БД</param>
        public static void InitializeDB(ILogger logger)
        {
            _logger = logger;
            try
            {
                using (CurrencyDbContext db = new CurrencyDbContext())
                {
                    db.Database.Migrate();
                    logger.LogInformation("Successfully applied migration");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }

        /// <summary>
        /// Метод для сохранения списка курсов валют в БД
        /// </summary>
        /// <param name="currencyRates">Список курсов валют</param>
        public static void SaveToDb(IEnumerable<CurrencyRate> currencyRates)
        {
            try
            {
                using (CurrencyDbContext db = new CurrencyDbContext())
                {
                    db.AddRange(currencyRates);
                    db.SaveChanges();
                    _logger.LogInformation("Successfully saved data to db");
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException is PostgresException postgresEx && postgresEx.SqlState == "23505")
                {
                    _logger.LogError("Записи с такой датой и ID валюты уже добавлены в базу");
                }
                else
                {
                    _logger.LogError(ex.Message);
                }
            }
        }
    }
}
