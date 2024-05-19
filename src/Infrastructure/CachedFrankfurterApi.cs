using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Infrastructure
{
    public class CachedFrankfurterApi(IFrankfurterApi api, IMemoryCache cache, ILogger<CachedFrankfurterApi> logger) : IFrankfurterApi
    {
        private const string DATE_FORMAT = "yyyy-MM-dd";
        private const int HISTORICAL_DAYS_AVAILABILITY = 90;

        public async Task<FrankfurterLatestResponse> GetLatestRatesAsync(string currency, CancellationToken cancellationToken)
        {
            if (cache.TryGetValue<FrankfurterLatestResponse>(currency, out var response))
            {
                logger.LogInformation($"Cache hit: {currency}");
                return response;
            }

            logger.LogInformation($"Cache miss: {currency}");

            response = await api.GetLatestRatesAsync(currency, cancellationToken).ConfigureAwait(false);
            cache.Set(currency, response, GetLatestRatesExpiry());
            return response;
        }

        // TODO: Set expiration to CET 16:00
        private static DateTimeOffset GetLatestRatesExpiry()
        {
            return new DateTimeOffset(DateTime.UtcNow.AddMinutes(5), new TimeSpan(0, 0, 0));
        }

        public async Task<FrankfurterHistoricalResponse> GetHistoricalRatesAsync(
            string currency, DateOnly fromDate, DateOnly toDate, CancellationToken cancellationToken)
        {
            var cacheKey = $"{currency}-{fromDate.ToString(DATE_FORMAT)}-{toDate.ToString(DATE_FORMAT)}";
            if (cache.TryGetValue<FrankfurterHistoricalResponse>(cacheKey, out var response))
            {
                logger.LogInformation($"Cache hit: {cacheKey}");
                return response;
            }

            logger.LogInformation($"Cache miss: {cacheKey}");

            response = await api.GetHistoricalRatesAsync(currency, fromDate, toDate, cancellationToken).ConfigureAwait(false);
            cache.Set(cacheKey, response, GetHistoricalRatesExpiry(toDate));
            return response;
        }

        private static DateTimeOffset GetHistoricalRatesExpiry(DateOnly toDate)
        {
            var expiryDate = toDate.AddDays(HISTORICAL_DAYS_AVAILABILITY + 1);
            var expiryDateTime = new DateTime(expiryDate.Year, expiryDate.Month, expiryDate.Day);
            return new DateTimeOffset(expiryDateTime, new TimeSpan(0, 0, 0));
        }
    }
}
