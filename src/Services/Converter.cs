using Infrastructure;
using Microsoft.Extensions.Logging;

namespace Services
{
    public class Converter(IFrankfurterApi api, ILogger<Converter> logger) : IConverter
    {
        private async Task<LatestRates> GetLatestRates(Currency currency, CancellationToken cancellationToken)
        {
            var response = await api.GetLatestRatesAsync(currency, cancellationToken);
            if (response is null)
            {
                return null;
            }

            return new LatestRates
            {
                Amount = response.Amount,
                BaseCurrency = new Currency(response.Base),
                Date = response.Date,
                Rates = response.Rates.ToRates(),
            };
        }

        public async Task<Result<LatestRates>> GetRatesAsync(Currency currency, CancellationToken cancellationToken)
        {
            try
            {
                var rates = await GetLatestRates(currency, cancellationToken);
                if (rates is null)
                {
                    return Result<LatestRates>.GetFailure("Unable to get data at this moment");
                }

                return Result<LatestRates>.GetSuccess(rates);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Error occurred");
                return Result<LatestRates>.GetFailure("Error occurred");
            }
        }

        public async Task<Result<LatestRates>> ConvertAsync(Currency currency, decimal amount, CancellationToken cancellationToken)
        {
            try
            {
                var rates = await GetLatestRates(currency, cancellationToken);
                if (rates is null)
                {
                    return Result<LatestRates>.GetFailure("Unable to get data at this moment");
                }

                var latestRates = rates.Convert(amount);
                return Result<LatestRates>.GetSuccess(latestRates);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Error occurred");
                return Result<LatestRates>.GetFailure("Error occurred");
            }
        }

        public async Task<Result<HistoricalRates>> GetHistoricalRatesAsync(
            Currency currency, DateOnly fromDate, DateOnly toDate, int page, int size,
            CancellationToken cancellationToken)
        {
            if (fromDate > toDate)
            {
                return Result<HistoricalRates>.GetFailure("From date can not be greater than To date");
            }

            if (fromDate.ToDateTime(TimeOnly.MinValue) < DateTime.Now.Date.AddDays(-90))
            {
                return Result<HistoricalRates>.GetFailure("From date can not be less than 90 days");
            }

            if (toDate.ToDateTime(TimeOnly.MinValue) > DateTime.Now.Date)
            {
                return Result<HistoricalRates>.GetFailure("To date can not be greater than today");
            }

            if (page < 1)
            {
                return Result<HistoricalRates>.GetFailure("Page can not be less than 1");
            }

            if (size < 1)
            {
                return Result<HistoricalRates>.GetFailure("Size can not be less than 1");
            }

            try
            {
                var historicalData = new HistoricalData(api);
                var result = await historicalData.GetRatesAsync(
                    currency, fromDate, toDate, page, size, cancellationToken);
                if (result is null)
                {
                    return Result<HistoricalRates>.GetFailure("Unable to get data at this moment");
                }

                return Result<HistoricalRates>.GetSuccess(result);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Error occurred");
                return Result<HistoricalRates>.GetFailure("Error occurred");
            }
        }
    }
}
