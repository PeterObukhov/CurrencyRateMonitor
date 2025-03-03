using CurrencyRateMonitor.Models;
using System.Globalization;
using System.Xml.Linq;

namespace CurrencyRateMonitor
{
    public class CurrencyService : ICurrencyService
    {
        private const string baseUrl = "http://www.cbr.ru/scripts/";
        private HttpClient client { get; set; }

        private struct CurrencyCode
        {
            public string Code;
            public string Name;
        }

        public CurrencyService() 
        { 
            client = new HttpClient();
        }

        public async Task<IEnumerable<CurrencyRate>> GetCurrentRatesAsync()
        {
            List<CurrencyRate> currencyRates = new List<CurrencyRate>();
            try
            {
                var response = await client.GetAsync($"{baseUrl}XML_daily.asp");
                XDocument xDoc = XDocument.Load(response.Content.ReadAsStream());
                var date = xDoc.Element("ValCurs").Attribute("Date").Value;
                foreach (var item in xDoc.Element("ValCurs").Elements())
                {
                    currencyRates.Add(new CurrencyRate
                    {
                        CurrencyId = item.Attribute("ID").Value,
                        CurrencyName = item.Element("Name").Value,
                        Date = DateOnly.Parse(date),
                        Nominal = int.Parse(item.Element("Nominal").Value),
                        Value = decimal.Parse(item.Element("Value").Value, CultureInfo.CreateSpecificCulture("ru-RU")),
                        VunitRate = decimal.Parse(item.Element("VunitRate").Value, CultureInfo.CreateSpecificCulture("ru-RU"))
                    });
                }
                return currencyRates;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return currencyRates;
            }
        }

        public async Task<IEnumerable<CurrencyRate>> GetLastMonthRatesAsync()
        {
            var currentDate = DateTime.Now.Date;
            var previousDate = currentDate.AddMonths(-1);
            List<CurrencyRate> currencyRates = new List<CurrencyRate>();
            try
            {
                foreach (var code in await GetCurrencyCodes())
                {
                    var response = await client.GetAsync($"{baseUrl}XML_dynamic.asp?date_req1={previousDate}&date_req2={currentDate}&VAL_NM_RQ={code.Code}");
                    XDocument xDoc = XDocument.Load(response.Content.ReadAsStream());
                    foreach (var item in xDoc.Element("ValCurs").Elements())
                    {
                        currencyRates.Add(new CurrencyRate
                        {
                            CurrencyId = code.Code,
                            CurrencyName = code.Name,
                            Date = DateOnly.Parse(item.Attribute("Date").Value),
                            Nominal = int.Parse(item.Element("Nominal").Value),
                            Value = decimal.Parse(item.Element("Value").Value, CultureInfo.CreateSpecificCulture("ru-RU")),
                            VunitRate = decimal.Parse(item.Element("VunitRate").Value, CultureInfo.CreateSpecificCulture("ru-RU"))
                        });
                    }
                }
                return currencyRates;
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return currencyRates;
            }
        }

        private async Task<IEnumerable<CurrencyCode>> GetCurrencyCodes()
        {
            List<CurrencyCode> currencyCodes = new List<CurrencyCode>();
            try
            {
                var response = await client.GetAsync($"{baseUrl}XML_val.asp?d=0");
                XDocument xDoc = XDocument.Load(response.Content.ReadAsStream());
                foreach (var item in xDoc.Element("Valuta").Elements())
                {
                    currencyCodes.Add(new CurrencyCode
                    {
                        Code = item.Attribute("ID").Value,
                        Name = item.Element("Name").Value
                    });
                }
                return currencyCodes;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return currencyCodes;
            }
        }
    }
}
