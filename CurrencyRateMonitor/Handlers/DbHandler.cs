using CurrencyRateMonitor.Models;
using Npgsql;
using CurrencyRateMonitor.Database;
using Microsoft.EntityFrameworkCore;

namespace CurrencyRateMonitor.Handlers
{
    public class DbHandler
    {
        public static void ApplyMigration()
        {
            try
            {
                using (CurrencyDbContext db = new CurrencyDbContext())
                {
                    db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static void SaveToDb(IEnumerable<CurrencyRate> currencyRates)
        {
            try
            {
                using (CurrencyDbContext db = new CurrencyDbContext())
                {
                    db.AddRange(currencyRates);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException is PostgresException postgresEx && postgresEx.SqlState == "23505")
                {
                    Console.WriteLine("Записи с такой датой и ID валюты уже добавлены в базу");
                }
                else
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
