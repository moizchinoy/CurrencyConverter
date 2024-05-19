using System.Net.Http.Json;

namespace Infrastructure
{
    public class FrankfurterApi(HttpClient httpClient) : IFrankfurterApi
    {
        public async Task<FrankfurterLatestResponse> GetLatestRatesAsync(string currency, CancellationToken cancellationToken)
        {
            var response = await httpClient
                .GetFromJsonAsync<FrankfurterLatestResponse>($"latest?from={currency}", cancellationToken)
                .ConfigureAwait(false);
            return response;
        }

        public async Task<FrankfurterHistoricalResponse> GetHistoricalRatesAsync(
            string currency, DateOnly fromDate, DateOnly toDate, CancellationToken cancellationToken)
        {
            var response = await httpClient
                .GetFromJsonAsync<FrankfurterHistoricalResponse>(
                    $"{fromDate:yyyy-MM-dd}..{toDate:yyyy-MM-dd}?from={currency}",
                    cancellationToken)
                .ConfigureAwait(false);
            return response;
        }
    }
}
