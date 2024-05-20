namespace Services
{
    public class LatestRates
    {
        public decimal Amount { get; init; }
        public Currency BaseCurrency { get; init; }
        public DateOnly Date { get; init; }
        public IEnumerable<CurrencyRate> Rates { get; init; }

        public LatestRates Convert(decimal amount)
        {
            return new LatestRates
            {
                Amount = amount,
                BaseCurrency = BaseCurrency,
                Date = Date,
                Rates = Rates.Select(r => new CurrencyRate
                {
                    Currency = r.Currency,
                    Value = r.Value * amount
                })
            };
        }
    }
}
