using CurrencyRateMonitor.Handlers;
using CurrencyRateMonitor.Models;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace CurrencyRateMonitor.Service
{
    /// <summary>
    /// Сервис для общения с API ЦБ
    /// </summary>
    public class CurrencyService
    {
        private const string baseUrl = "http://www.cbr.ru/scripts/";
        private static HttpClient client;
        private static ILogger _logger;


        /// <summary>
        /// Инициализация сервиса
        /// </summary>
        /// <param name="logger">Логгер</param>
        public static void InitializeService(ILogger logger)
        {
            client = new HttpClient();
            _logger = logger;
        }

        /// <summary>
        /// Получить данные за текущий день
        /// </summary>
        /// <returns>Список значений курса валют</returns>
        public static async Task<IEnumerable<CurrencyRate>> GetCurrentRatesAsync()
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
                        Date = DateOnly.Parse(date),
                        Nominal = int.Parse(item.Element("Nominal").Value),
                        Value = decimal.Parse(item.Element("Value").Value, CultureInfo.CreateSpecificCulture("ru-RU")),
                        VunitRate = decimal.Parse(item.Element("VunitRate").Value, CultureInfo.CreateSpecificCulture("ru-RU"))
                    });
                }
                _logger.LogInformation("Successfully retrieved current rates");
                return currencyRates;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return currencyRates;
            }
        }

        /// <summary>
        /// Получить значения курса валют за последний месяц
        /// </summary>
        /// <returns>Список значений курса валют</returns>
        public static async Task<IEnumerable<CurrencyRate>> GetLastMonthRatesAsync()
        {
            List<CurrencyRate> currencyRates = new List<CurrencyRate>();
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
            DateOnly lastDateInDb = DbHandler.GetLastCurrencyRateDate();
            if (lastDateInDb != currentDate)
            {
                DateOnly previousDate;
                if (lastDateInDb != DateOnly.MinValue)
                {
                    previousDate = lastDateInDb;
                }
                else
                {
                    previousDate = currentDate.AddMonths(-1);
                }

                try
                {
                    foreach (var code in DbHandler.GetCurrencyCodes())
                    {
                        var previousDateStr = previousDate.ToString("dd.MM.yyyy");
                        var currentDateStr = currentDate.ToString("dd.MM.yyyy");
                        var response = await client.GetAsync($"{baseUrl}XML_dynamic.asp?date_req1={previousDateStr}&date_req2={currentDateStr}&VAL_NM_RQ={code}");
                        XDocument xDoc = XDocument.Load(response.Content.ReadAsStream());
                        foreach (var item in xDoc.Element("ValCurs").Elements())
                        {
                            currencyRates.Add(new CurrencyRate
                            {
                                CurrencyId = code,
                                Date = DateOnly.Parse(item.Attribute("Date").Value, CultureInfo.CreateSpecificCulture("ru-RU")),
                                Nominal = int.Parse(item.Element("Nominal").Value),
                                Value = decimal.Parse(item.Element("Value").Value, CultureInfo.CreateSpecificCulture("ru-RU")),
                                VunitRate = decimal.Parse(item.Element("VunitRate").Value, CultureInfo.CreateSpecificCulture("ru-RU"))
                            });
                        }
                    }
                    _logger.LogInformation("Successfully retrieved monthly rates");
                    return currencyRates;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return currencyRates;
                }
            }

            return currencyRates;
        }

        /// <summary>
        /// Получить коды валют
        /// </summary>
        /// <returns>Список кодов валют с их названиями</returns>
        public static async Task<IEnumerable<CurrencyCode>> GetCurrencyCodes()
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
                _logger.LogInformation("Successfully retrieved currency codes");
                return currencyCodes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return currencyCodes;
            }
        }
    }
}
