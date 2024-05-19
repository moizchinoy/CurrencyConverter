using Infrastructure;
using Microsoft.Extensions.Logging;

namespace Services
{
    public interface IExchangeRatesManager
    {
        Task<Result<ExchangeRates>> GetRatesAsync(Currency currency, CancellationToken cancellationToken);

        Task<Result<ExchangeRates>> ConvertAsync(Currency currency, decimal amount, CancellationToken cancellationToken);

        Task<Result<HistoricalExchangeRates>> GetHistoricalRatesAsync(
            Currency currency, DateOnly fromDate, DateOnly toDate, int page, int size, CancellationToken cancellationToken);
    }

    public class ExchangeRatesManager(IFrankfurterApi api, ILogger<ExchangeRatesManager> logger) : IExchangeRatesManager
    {
        private async Task<ExchangeRates> GetLatestRates(Currency currency, CancellationToken cancellationToken)
        {
            var apiResponse = await api.GetLatestRatesAsync(currency.ToString(), cancellationToken);

            return new ExchangeRates(apiResponse.Amount, apiResponse.Base,
                apiResponse.Date, apiResponse.Rates);
        }

        public async Task<Result<ExchangeRates>> GetRatesAsync(Currency currency, CancellationToken cancellationToken)
        {
            try
            {
                var exchangeRates = await GetLatestRates(currency, cancellationToken);
                if (exchangeRates is not null)
                {
                    return Result<ExchangeRates>.GetSuccess(exchangeRates);
                }
                else
                {
                    return Result<ExchangeRates>.GetFailure("Unable to get data at this moment");
                }
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Error occurred");
                return Result<ExchangeRates>.GetFailure("Error occurred");
            }
        }

        public async Task<Result<ExchangeRates>> ConvertAsync(Currency currency, decimal amount, CancellationToken cancellationToken)
        {
            try
            {
                var exchangeRates = await GetLatestRates(currency, cancellationToken);
                if (exchangeRates is not null)
                {
                    var latestRates = exchangeRates.Convert(amount);
                    return Result<ExchangeRates>.GetSuccess(latestRates);
                }
                else
                {
                    return Result<ExchangeRates>.GetFailure("Unable to get data at this moment");
                }
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Error occurred");
                return Result<ExchangeRates>.GetFailure("Error occurred");
            }
        }

        public async Task<Result<HistoricalExchangeRates>> GetHistoricalRatesAsync(
            Currency currency, DateOnly fromDate, DateOnly toDate, int page, int size, 
            CancellationToken cancellationToken)
        {
            if (fromDate > toDate)
            {
                return Result<HistoricalExchangeRates>.GetFailure(
                    "From date can not be greater than To date");
            }

            if (fromDate.ToDateTime(TimeOnly.MinValue) < DateTime.Now.Date.AddDays(-90))
            {
                return Result<HistoricalExchangeRates>.GetFailure(
                    "From date can not be less than 90 days");
            }

            if (toDate.ToDateTime(TimeOnly.MinValue) > DateTime.Now.Date)
            {
                return Result<HistoricalExchangeRates>.GetFailure(
                    "To date can not be greater than today");
            }

            try
            {
                var historicalData = new HistoricalData(api);
                var result = await historicalData.GetHistoricalRatesAsync(
                    currency, fromDate, toDate, page, size, cancellationToken);
                if (result is not null)
                {
                    return Result<HistoricalExchangeRates>.GetSuccess(result);
                }
                else
                {
                    return Result<HistoricalExchangeRates>.GetFailure(
                        "Unable to get data at this moment");
                }
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Error occurred");
                return Result<HistoricalExchangeRates>.GetFailure("Error occurred");
            }
        }
    }
}
