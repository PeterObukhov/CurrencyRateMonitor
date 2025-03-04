using CurrencyRateMonitor.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection.Metadata;
using Npgsql;
using CurrencyRateMonitor.Database;

namespace CurrencyRateMonitor.Handlers
{
    public class DbHandler
    {
        public static void ApplyMigration()
        {
            using (CurrencyDbContext db = new CurrencyDbContext())
            {
                db.Database.Migrate();
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
                    Console.WriteLine(ex.InnerException.Message);
                }
            }
        }
    }
}
