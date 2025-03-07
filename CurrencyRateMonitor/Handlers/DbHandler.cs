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
        public static void SaveCurrencyRatesToDb(IEnumerable<CurrencyRate> currencyRates)
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
                    _logger.LogWarning("Записи с такой датой и ID валюты уже добавлены в базу");
                }
                else
                {
                    _logger.LogError(ex.Message);
                }
            }
        }

        /// <summary>
        /// Метод для сохранения кодов валют в БД
        /// </summary>
        /// <param name="currencyRates">Список кодов валют</param>
        public static void SaveCurrencyCodesToDb(IEnumerable<CurrencyCode> currencyCodes)
        {
            try
            {
                using (CurrencyDbContext db = new CurrencyDbContext())
                {
                    db.AddRange(currencyCodes);
                    db.SaveChanges();
                    _logger.LogInformation("Successfully saved data to db");
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException is PostgresException postgresEx && postgresEx.SqlState == "23505")
                {
                    _logger.LogWarning("Записи с таким ID валюты уже добавлены в базу");
                }
                else
                {
                    _logger.LogError("Ошибка сохранения данных в БД: " + ex.Message);
                }
            }
        }

        public static DateOnly GetLastCurrencyRateDate()
        {
            try
            {
                using (CurrencyDbContext db = new CurrencyDbContext())
                {
                    var dates = db.CurrencyRates.Select(x => x.Date);
                    if (dates.Count() > 0)
                    {
                        return db.CurrencyRates.Select(x => x.Date).Max();
                    }
                    else
                    {
                        return DateOnly.MinValue;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Ошибка чтения даты из БД" + ex.Message);
                return DateOnly.MinValue;
            }
        }

        public static List<string> GetCurrencyCodes()
        {
            try
            {
                using (CurrencyDbContext db = new CurrencyDbContext())
                {
                    return db.CurrencyCodes.Select(x => x.Code).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Ошибка чтения кодов из БД" + ex.Message);
                return null;
            }
        }
    }
}
