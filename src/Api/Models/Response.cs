using Services.Models;

namespace Api.Models
{
    public class Response(LatestRates latestRates)
    {
        public decimal Amount { get; } = latestRates.Amount;
        public string BaseCurrency { get; } = latestRates.BaseCurrency;
        public DateOnly Date { get; } = latestRates.Date;
        public object Rates { get; } = latestRates.Rates.Select(x => new
        {
            Currency = x.Currency.ToString(),
            x.Value,
        });
    }
}
