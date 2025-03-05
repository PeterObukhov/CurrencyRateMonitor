namespace CurrencyRateMonitor.Models
{
    /// <summary>
    /// Модель курса валюты
    /// </summary>
    public class CurrencyRate
    {
        public int Id { get; set; }
        
        /// <summary>
        /// Дата записи курса
        /// </summary>
        public DateOnly Date { get; set; }

        /// <summary>
        /// Id валюты от ЦБ
        /// </summary>
        public string CurrencyId { get; set; }

        /// <summary>
        /// Название валюты на русском языке
        /// </summary>
        public string CurrencyName { get; set; }
        
        /// <summary>
        /// Номинал валюты
        /// </summary>
        public int Nominal { get; set; }
        
        /// <summary>
        /// Значение курса
        /// </summary>
        public decimal Value { get; set; }
        
        /// <summary>
        /// Знчение за одну единицу валюты
        /// </summary>
        public decimal VunitRate { get; set; }
    }
}
