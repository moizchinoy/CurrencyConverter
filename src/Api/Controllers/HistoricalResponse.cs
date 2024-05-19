using Services;

namespace Api.Controllers
{
    public class HistoricalResponse(HistoricalExchangeRates historicalExchangeRates)
    {
        public decimal Amount { get; } = historicalExchangeRates.Amount;
        public string BaseCurrency { get; } = historicalExchangeRates.BaseCurrency.ToString();
        public DateOnly FromDate { get; } = historicalExchangeRates.FromDate;
        public DateOnly ToDate { get; } = historicalExchangeRates.ToDate;
        public object DailyRates { get; } = historicalExchangeRates.Rates.Select(x => new
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
            PreviousPage = historicalExchangeRates.PreviousPage,
            NextPage = historicalExchangeRates.NextPage,
        };
    }
}
