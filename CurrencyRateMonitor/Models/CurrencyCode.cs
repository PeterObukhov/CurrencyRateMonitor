using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurrencyRateMonitor.Models
{
    /// <summary>
    /// Код и название валюты
    /// </summary>
    [Table("CurrencyCodes")]
    [PrimaryKey("Code")]
    public class CurrencyCode
    {
        public string Code {  get; set; }
        public string Name { get; set; }
        public List<CurrencyRate>? Rates { get; set; } = new();
    }
}
