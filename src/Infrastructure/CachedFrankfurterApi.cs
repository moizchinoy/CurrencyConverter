using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure
{
    public class CachedFrankfurterApi : IFrankfurterApi
    {
        private readonly IFrankfurterApi _frankfurterApi;
        private readonly IMemoryCache _memoryCache;

        public CachedFrankfurterApi(IFrankfurterApi frankfurterApi, IMemoryCache memoryCache)
        {
            _frankfurterApi = frankfurterApi;
            _memoryCache = memoryCache;
        }

        public async Task<FrankfurterResponse> GetLatestRates(string currency)
        {
            if (_memoryCache.TryGetValue<FrankfurterResponse>(currency, out var response))
            {
                return response;
            }

            response = await _frankfurterApi.GetLatestRates(currency);
            _memoryCache.Set(currency, response);
            return response;
        }
    }
}
