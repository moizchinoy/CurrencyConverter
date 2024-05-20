namespace Services
{
    public class ExchangeRates
    {
        public decimal Amount { get; init; }
        public Currency BaseCurrency { get; init; }
        public DateOnly Date { get; init; }
        public IEnumerable<Rate> Rates { get; init; }

        public ExchangeRates Convert(decimal amount)
        {
            return new ExchangeRates
            {
                Amount = amount,
                BaseCurrency = BaseCurrency,
                Date = Date,
                Rates = Rates.Select(r => new Rate
                {
                    Currency = r.Currency,
                    Value = r.Value * amount
                })
            };
        }
    }
}
