using Services.Models;

namespace Services.Extensions
{
    public static class CurrencyRateExtensions
    {
        public static IEnumerable<CurrencyRate> ToRates(this Dictionary<string, decimal> dict)
        {
            return dict.Select(x => new CurrencyRate
            {
                Currency = new Currency(x.Key),
                Value = x.Value
            });
        }
    }
}
