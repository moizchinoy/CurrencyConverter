namespace Services
{
    public static class Extensions
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
