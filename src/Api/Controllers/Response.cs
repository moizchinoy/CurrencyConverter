using Services;

namespace Api.Controllers
{
    public class Response(ExchangeRates exchangeRates)
    {
        public decimal Amount { get; } = exchangeRates.Amount;
        public string BaseCurrency { get; } = exchangeRates.BaseCurrency.ToString();
        public DateOnly Date { get; } = exchangeRates.Date;
        public object Rates { get; } = exchangeRates.Rates.Select(x => new
        {
            Currency = x.Currency.ToString(),
            x.Value,
        });
    }
}
