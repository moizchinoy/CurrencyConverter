namespace Services
{
    public class ExchangeRates
    {
        public ExchangeRates(decimal amount, Currency baseCurrency, DateOnly date, IEnumerable<Rate> rates)
        {
            Amount = amount;
            BaseCurrency = baseCurrency;
            Date = date;
            Rates = rates;
        }

        public ExchangeRates(decimal amount, string baseCurrency, DateOnly date, Dictionary<string, decimal> rates)
        {
            Amount = amount;
            BaseCurrency = new Currency(baseCurrency);
            Date = date;
            Rates = rates.Select(x => new Rate(new Currency(x.Key), x.Value));
        }

        public decimal Amount { get; }
        public Currency BaseCurrency { get; }
        public DateOnly Date { get; }
        public IEnumerable<Rate> Rates { get; }

        public ExchangeRates Convert(decimal amount)
        {
            return new ExchangeRates(
                amount,
                BaseCurrency,
                Date,
                Rates.Select(r => new Rate(r.Currency, r.Value * amount)));
        }
    }
}
