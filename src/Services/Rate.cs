using System.Runtime.CompilerServices;

namespace Services
{
    public static class RatesExtensions
    {
        public static IEnumerable<Rate> ToRates(this Dictionary<string, decimal> dict)
        {
            return dict.Select(x => new Rate
            {
                Currency = new Currency(x.Key),
                Value = x.Value
            });
        }
    }

    public class Rate
    {
        public Currency Currency { get; init; }
        public decimal Value { get; init; }
    }
}
