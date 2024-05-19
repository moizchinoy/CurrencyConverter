using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infrastructure
{
    public class FrankfurterResponse
    {
        public decimal Amount { get; set; }
        public string Base { get; set; }
        public DateOnly Date { get; set; }
        public Rates Rates { get; set; }
    }

    public class Rates
    {
        public decimal AUD { get; set; }
        public decimal BGN { get; set; }
        public decimal BRL { get; set; }
        public decimal CAD { get; set; }
        public decimal CHF { get; set; }
        public decimal CNY { get; set; }
        public decimal CZK { get; set; }
        public decimal EUR { get; set; }
        public decimal DKK { get; set; }
        public decimal GBP { get; set; }
        public decimal HKD { get; set; }
        public decimal HUF { get; set; }
        public decimal IDR { get; set; }
        public decimal ILS { get; set; }
        public decimal INR { get; set; }
        public decimal ISK { get; set; }
        public decimal JPY { get; set; }
        public decimal KRW { get; set; }
        public decimal MXN { get; set; }
        public decimal MYR { get; set; }
        public decimal NOK { get; set; }
        public decimal NZD { get; set; }
        public decimal PHP { get; set; }
        public decimal PLN { get; set; }
        public decimal RON { get; set; }
        public decimal SEK { get; set; }
        public decimal SGD { get; set; }
        public decimal THB { get; set; }
        public decimal TRY { get; set; }
        public decimal USD { get; set; }
        public decimal ZAR { get; set; }

        public Dictionary<string, decimal> GetDictionary()
        {
            return new Dictionary<string, decimal>
            {
                { "AUD", AUD },
                { "BGN", BGN },
                { "BRL", BRL },
                { "CAD", CAD },
                { "CHF", CHF },
                { "CNY", CNY },
                { "CZK", CZK },
                { "EUR", EUR },
                { "DKK", DKK },
                { "GBP", GBP },
                { "HKD", HKD },
                { "HUF", HUF },
                { "IDR", IDR },
                { "ILS", ILS },
                { "INR", INR },
                { "ISK", ISK },
                { "JPY", JPY },
                { "KRW", KRW },
                { "MXN", MXN },
                { "MYR", MYR },
                { "NOK", NOK },
                { "NZD", NZD },
                { "PHP", PHP },
                { "PLN", PLN },
                { "RON", RON },
                { "SEK", SEK },
                { "SGD", SGD },
                { "THB", THB },
                { "TRY", TRY },
                { "USD", USD },
                { "ZAR", ZAR }
            };
        }
    }

    public interface IFrankfurterApi
    {
        Task<FrankfurterResponse> GetLatestRates(string currency);
    }

    public class FrankfurterApi : IFrankfurterApi
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public FrankfurterApi(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<FrankfurterResponse> GetLatestRates(string currency)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetFromJsonAsync<FrankfurterResponse>(
                $"https://api.frankfurter.app/latest?from={currency}");
            return response;
            //.GetAsync($"https://api.frankfurter.app/latest?from={currency}");

            //response.EnsureSuccessStatusCode();
            //var str = await response.Content.ReadAsStringAsync();
            //return JsonSerializer.Deserialize<FrankfurterResponse>(str);
        }
    }
}
