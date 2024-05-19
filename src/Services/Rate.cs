namespace Services
{
    public class Rate(Currency currency, decimal value)
    {
        public Currency Currency { get; } = currency;
        public decimal Value { get; } = value;
    }
}
