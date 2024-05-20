namespace Services
{
    public class HistoricalRates
    {
        public decimal Amount { get; init; }
        public Currency BaseCurrency { get; init; }
        public DateOnly FromDate { get; init; }
        public DateOnly ToDate { get; init; }
        public Dictionary<DateOnly, IEnumerable<CurrencyRate>> DailyRates { get; init; }
        public int? PreviousPage { get; init; }
        public int? NextPage { get; init; }
    }
}
