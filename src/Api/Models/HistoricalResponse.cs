using Services.Models;

namespace Api.Models
{
    public class HistoricalResponse(HistoricalRates historicalRates)
    {
        public decimal Amount { get; } = historicalRates.Amount;
        public string BaseCurrency { get; } = historicalRates.BaseCurrency;
        public DateOnly FromDate { get; } = historicalRates.FromDate;
        public DateOnly ToDate { get; } = historicalRates.ToDate;
        public object DailyRates { get; } = historicalRates.DailyRates.Select(x => new
        {
            Date = x.Key,
            Rates = x.Value.Select(x => new
            {
                Currency = x.Currency.ToString(),
                x.Value,
            })
        });
        public object _links { get; } = new
        {
            historicalRates.PreviousPage,
            historicalRates.NextPage,
        };
    }
}
