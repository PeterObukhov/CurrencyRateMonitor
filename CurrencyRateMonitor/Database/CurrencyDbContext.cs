using CurrencyRateMonitor.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CurrencyRateMonitor.Database
{
    public class CurrencyDbContext : DbContext
    {
        public DbSet<CurrencyRate> CurrencyRates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json");

            IConfiguration config = builder.Build();

            var connectionString = $"Host={config.GetSection("Host").Value};" +
                $"Port={config.GetSection("Port").Value};" +
                $"Database={config.GetSection("Database").Value};" +
                $"Username={config.GetSection("Username").Value};" +
                $"Password={config.GetSection("Password").Value}";

            optionsBuilder.UseNpgsql(connectionString);
        }
    }
}
