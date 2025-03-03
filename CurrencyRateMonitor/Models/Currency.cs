namespace CurrencyRateMonitor.Models
{
    public class CurrencyRate
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public string CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public int Nominal { get; set; }
        public decimal Value { get; set; }
        public decimal VunitRate { get; set; }
    }
}
