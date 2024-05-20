using Infrastructure;
using Services.Models;

namespace Services
{
    public class FilteredConverter : IConverter
    {
        private readonly IConverter _converter;
        private readonly IEnumerable<Currency> _restrictedCurrencies;

        public FilteredConverter(IConverter converter, IEnumerable<Currency> restrictedCurrencies)
        {
            _converter = converter;
            _restrictedCurrencies = restrictedCurrencies;
        }

        public async Task<Result<LatestRates>> GetRatesAsync(Currency currency, CancellationToken cancellationToken)
        {
            return await _converter
                .GetRatesAsync(currency, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<Result<LatestRates>> ConvertAsync(Currency currency, decimal amount, CancellationToken cancellationToken)
        {
            if (_restrictedCurrencies.Contains(currency))
            {
                return Result<LatestRates>.GetFailure("Not Allowed");
            }

            var response = await _converter
                .ConvertAsync(currency, amount, cancellationToken)
                .ConfigureAwait(false);
            if (!response.IsSuccess)
            {
                return response;
            }

            return Result<LatestRates>.GetSuccess(new LatestRates
            {
                Amount = response.Value.Amount,
                BaseCurrency = response.Value.BaseCurrency,
                Date = response.Value.Date,
                Rates = response.Value.Rates.Where(r => !_restrictedCurrencies.Contains(r.Currency))
            });
        }

        public async Task<Result<HistoricalRates>> GetHistoricalRatesAsync(
            Currency currency, DateOnly fromDate, DateOnly toDate, int page, int size,
            CancellationToken cancellationToken)
        {
            return await _converter
                .GetHistoricalRatesAsync(currency, fromDate, toDate, page, size, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
