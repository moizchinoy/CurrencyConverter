namespace Services
{
    public class Currency : IEquatable<Currency>
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

        public static implicit operator string(Currency currency) => currency.ToString();

        public static bool operator ==(Currency a, Currency b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Currency a, Currency b)
        {
            return a.Equals(b);
        }

        public override bool Equals(object obj)
        {
            return obj is Currency currency && Equals(currency);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_value);
        }

        public override string ToString()
        {
            return _value;
        }

        public bool Equals(Currency currency)
        {
            return _value == currency._value;
        }
    }
}
