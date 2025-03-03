using CurrencyRateMonitor.Models;
using System.Xml.Serialization;

namespace CurrencyRateMonitor
{
    public class CurrencyService
    {
        private const string baseUrl = "http://www.cbr.ru/scripts/XML_daily.asp";
        public async Task<CurrencyRates> GetCurrentRates()
        {
            using (HttpClient client = new HttpClient())
            {
                var currentDate = DateTime.Now.Date.ToString("dd/MM/yyyy");
                var response = await client.GetAsync($"{baseUrl}?date_req={currentDate}");
                var serializer = new XmlSerializer(typeof(CurrencyRates));
                return serializer.Deserialize(response.Content.ReadAsStream()) as CurrencyRates;
            }
        }
    }
}
