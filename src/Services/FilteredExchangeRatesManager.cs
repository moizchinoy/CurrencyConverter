using Infrastructure;

namespace Services
{
    public class FilteredExchangeRatesManager : IExchangeRatesManager
    {
        private readonly IExchangeRatesManager _exchangeRatesManager;
        private readonly IEnumerable<Currency> _restrictedCurrencies;

        public FilteredExchangeRatesManager(IExchangeRatesManager exchangeRatesManager, IEnumerable<Currency> restrictedCurrencies)
        {
            _exchangeRatesManager = exchangeRatesManager;
            _restrictedCurrencies = restrictedCurrencies;
        }

        public async Task<Result<ExchangeRates>> GetRatesAsync(Currency currency, CancellationToken cancellationToken)
        {
            return await _exchangeRatesManager
                .GetRatesAsync(currency, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<Result<ExchangeRates>> ConvertAsync(Currency currency, decimal amount, CancellationToken cancellationToken)
        {
            if (_restrictedCurrencies.Contains(currency))
            {
                return Result<ExchangeRates>.GetFailure("Not Allowed");
            }

            var response = await _exchangeRatesManager
                .ConvertAsync(currency, amount, cancellationToken)
                .ConfigureAwait(false);
            if (!response.IsSuccess)
            {
                return response;
            }

            return Result<ExchangeRates>.GetSuccess(new ExchangeRates
            {
                Amount = response.Value.Amount,
                BaseCurrency = response.Value.BaseCurrency,
                Date = response.Value.Date,
                Rates = response.Value.Rates.Where(r => !_restrictedCurrencies.Contains(r.Currency))
            });
        }

        public async Task<Result<HistoricalExchangeRates>> GetHistoricalRatesAsync(
            Currency currency, DateOnly fromDate, DateOnly toDate, int page, int size,
            CancellationToken cancellationToken)
        {
            return await _exchangeRatesManager
                .GetHistoricalRatesAsync(currency, fromDate, toDate, page, size, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
