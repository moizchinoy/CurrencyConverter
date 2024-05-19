namespace Infrastructure
{
    public class FrankfurterLatestResponse
    {
        public decimal Amount { get; set; }
        public string Base { get; set; }
        public DateOnly Date { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }
    }

    public class FrankfurterHistoricalResponse
    {
        public decimal Amount { get; set; }
        public string Base { get; set; }
        public DateOnly Start_Date { get; set; }
        public DateOnly End_Date { get; set; }
        public Dictionary<DateOnly, Dictionary<string, decimal>> Rates { get; set; }
    }

    public interface IFrankfurterApi
    {
        Task<FrankfurterLatestResponse> GetLatestRatesAsync(string currency, CancellationToken cancellationToken);
        Task<FrankfurterHistoricalResponse> GetHistoricalRatesAsync(
            string currency, DateOnly fromDate, DateOnly toDate, CancellationToken cancellationToken);
    }
}
