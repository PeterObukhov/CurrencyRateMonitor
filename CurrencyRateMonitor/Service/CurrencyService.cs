﻿using CurrencyRateMonitor.Models;
using System.Globalization;
using System.Xml.Linq;

namespace CurrencyRateMonitor.Service
{
    public static class CurrencyService
    {
        private const string baseUrl = "http://www.cbr.ru/scripts/";
        private static HttpClient client { get; set; }

        private struct CurrencyCode
        {
            public string Code;
            public string Name;
        }

        public static void InitializeService()
        {
            client = new HttpClient();
        }

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

        public static async Task<IEnumerable<CurrencyRate>> GetLastMonthRatesAsync()
        {
            var currentDate = DateTime.Now.Date;
            var previousDate = currentDate.AddMonths(-1);
            List<CurrencyRate> currencyRates = new List<CurrencyRate>();
            try
            {
                foreach (var code in await GetCurrencyCodes())
                {
                    var previousDateStr = previousDate.ToString("dd.MM.yyyy");
                    var currentDateStr = currentDate.ToString("dd.MM.yyyy");
                    var response = await client.GetAsync($"{baseUrl}XML_dynamic.asp?date_req1={previousDateStr}&date_req2={currentDateStr}&VAL_NM_RQ={code.Code}");
                    XDocument xDoc = XDocument.Load(response.Content.ReadAsStream());
                    foreach (var item in xDoc.Element("ValCurs").Elements())
                    {
                        currencyRates.Add(new CurrencyRate
                        {
                            CurrencyId = code.Code,
                            CurrencyName = code.Name,
                            Date = DateOnly.Parse(item.Attribute("Date").Value, CultureInfo.CreateSpecificCulture("ru-RU")),
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

        private static async Task<IEnumerable<CurrencyCode>> GetCurrencyCodes()
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
