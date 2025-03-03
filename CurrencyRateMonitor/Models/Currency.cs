using System.Globalization;
using System.Xml.Serialization;

namespace CurrencyRateMonitor.Models
{
    public class Currency
    {
        [XmlAttribute]
        public string ID { get; set; }
        public int NumCode { get; set; }
        public string CharCode { get; set; }
        public int Nominal {  get; set; }
        public string Name { get; set; }

        //https://stackoverflow.com/questions/17437946/xml-deserialization-crashes-on-decimal-parse-due-to-formatting
        [XmlIgnore]
        public decimal CurrencyValue { get; set; } 
        
        [XmlElement("Value")]
        public string ValueString
        {
            get
            {
                return CurrencyValue.ToString();
            }
            set 
            {
                decimal currencyValue;
                Decimal.TryParse(value, NumberStyles.Any, CultureInfo.CreateSpecificCulture("ru-RU"), out currencyValue);
                CurrencyValue = currencyValue;
            } 
        }

        [XmlIgnore]
        public decimal VunitRate { get; set; } 
        
        [XmlElement("VunitRate")]
        public string VunitRateString 
        { 
            get
            {
                return VunitRate.ToString();
            }
            set 
            {
                decimal vunitRate;
                Decimal.TryParse(value, NumberStyles.Any, CultureInfo.CreateSpecificCulture("ru-RU"), out vunitRate);
                VunitRate = vunitRate;
            } 
        }
    }

    [XmlRoot("ValCurs")]
    public class CurrencyRates
    {
        [XmlElement("Valute", typeof(Currency))]
        public List<Currency> ValCurs { get; set; }
    }
}
