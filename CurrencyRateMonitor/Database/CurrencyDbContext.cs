using CurrencyRateMonitor.Handlers;
using CurrencyRateMonitor.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyRateMonitor.Database
{
    public class CurrencyDbContext : DbContext
    {
        public DbSet<CurrencyRate> CurrencyRates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { 
            var connectionString = $"Host={ConfigurationHandler.Сonfiguration.GetSection("Host").Value};" +
                $"Port={ConfigurationHandler.Сonfiguration.GetSection("Port").Value};" +
                $"Database={ConfigurationHandler.Сonfiguration.GetSection("Database").Value};" +
                $"Username={ConfigurationHandler.Сonfiguration.GetSection("Username").Value};" +
                $"Password={ConfigurationHandler.Сonfiguration.GetSection("Password").Value}";

            optionsBuilder.UseNpgsql(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CurrencyRate>().HasIndex(rate => new { rate.CurrencyId, rate.Date }).IsUnique();
        }
    }
}
