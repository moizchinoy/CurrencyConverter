namespace Services
{
    public class Currency
    {
        private readonly string _value;

        public Currency(string currency)
        {
            if (string.IsNullOrWhiteSpace(currency))
            {
                throw new ArgumentException($"'{nameof(currency)}' cannot be null or whitespace.", nameof(currency));
            }

            _value = currency.ToUpper();
        }

        public override bool Equals(object obj)
        {
            return obj is Currency currency &&
                   _value == currency._value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_value);
        }
    }

    public class ExchangeRates(decimal amount, string baseCurrency, DateOnly date, Dictionary<string, decimal> rates)
    {
        public decimal Amount { get; } = amount;
        public string BaseCurrency { get; } = baseCurrency;
        public DateOnly Date { get; } = date;
        public Dictionary<string, decimal> Rates { get; } = rates;

        public ExchangeRates Convert(decimal amount)
        {
            var newRates = new Dictionary<string, decimal>(Rates.Select(r =>
                new KeyValuePair<string, decimal>(r.Key, r.Value * amount)));

            return new ExchangeRates(amount, BaseCurrency, Date, newRates);
        }
    }
}
